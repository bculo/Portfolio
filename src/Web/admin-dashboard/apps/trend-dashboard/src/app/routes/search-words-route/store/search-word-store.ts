import {
    patchState,
    signalStore,
    withMethods,
    withState,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { inject } from '@angular/core';
import { filter, map, pipe, switchMap, tap, zip } from 'rxjs';
import { tapResponse } from '@ngrx/operators';
import { withDevtools } from '@angular-architects/ngrx-toolkit'
import { SearchWordService } from '../../../shared/services/open-api';
import { SearchWordFilterModel, SearchWordItem, SearchWordNewItem, SearchWordStats } from './search-word.model';
import { mapToFilterReqDto, mapToFilterViewModel, mapToSyncStatsViewModel } from '../mappers/mapper';
import { removeEntity, setAllEntities, updateEntity, withEntities } from '@ngrx/signals/entities';
import { ActiveEnumOptions } from '../../../shared/enums/enums';
import { SideModalService } from '../../../shared/components/side-modal/side-modal.service';
import { Router } from '@angular/router';

interface SearchWordState {
    filterHash: string | null,
    filter: SearchWordFilterModel | null,
    searchWordModal: string,
    searchWordModalSyncStats: SearchWordStats | null,
    newItem: SearchWordNewItem | null,
    updateItem: SearchWordItem | null,
    isLoading: boolean,
    totalCount: number
    creatingItem: boolean;
}

const initialState: SearchWordState = {
    newItem: null,
    searchWordModalSyncStats: null,
    filterHash: null,
    filter: null,
    searchWordModal: 'search-word-modal',
    isLoading: false,
    totalCount: 0,
    updateItem: null,
    creatingItem: false
}


export const SearchWordStore = signalStore(
    { providedIn: 'root' },
    withEntities<SearchWordItem>(),
    withState(initialState),
    withDevtools('search-word'),
    withMethods((store, wordService = inject(SearchWordService), modalService = inject(SideModalService), router = inject(Router)) => ({

        fetch: rxMethod<SearchWordFilterModel>(
            pipe(
                filter((filter) => JSON.stringify(filter) !== store.filterHash()),
                tap((filter) => patchState(store, {filter: filter, filterHash: JSON.stringify(filter), isLoading: true })),
                map(mapToFilterReqDto),
                switchMap((dto) => 
                    wordService.filterSearchWords(dto.active, dto.contextType, dto.searchEngine, dto.query ?? '', dto.sort, dto.page, dto.take).pipe(
                            tapResponse({
                                next: (response) => {
                                    patchState(store, setAllEntities(mapToFilterViewModel(response)));
                                    patchState(store, { totalCount: response.count })
                                },
                                error: console.error,
                                finalize: () => patchState(store, { isLoading: false })
                            })
                        ),  
                )
            )  
        ),

        createWord: rxMethod<void>(
            pipe(
                tap(x => patchState(store, { creatingItem: true })),
                map(x => store.newItem()),
                switchMap((item) => wordService.addNewSearchWord(
                    { 
                        searchWord: item!.searchWord ?? '', 
                        contextType: item!.contextType,
                        searchEngine: item!.searchEngine,
                    })
                    .pipe(tapResponse({
                        next: (item) => {
                            const copy = { ...store.newItem(), wordId: item.id } as SearchWordNewItem;
                            patchState(store, { newItem: copy });
                        },
                        error: (_) => patchState(store, { creatingItem: true }),
                      }))
                ),
                switchMap((res) => wordService.attachImage(res.id!, store.newItem()?.file!).pipe(
                    tapResponse({
                        next: (_) => { router.navigate(["/words"]) },
                        error: (_) => patchState(store, { creatingItem: true }),
                        finalize: () => patchState(store, { newItem: null, creatingItem: false })
                    })
                ))
            )
        ),


        storeItemLocally(newItem: SearchWordNewItem) {
            patchState(store, { newItem: newItem} );
        },

        attachImageToLocalItem(image: File) {
            if(!store.newItem()) return;
            const item = { ...store.newItem(), file: image } as SearchWordNewItem
            patchState(store, { newItem: item } );
        },

        removeLocalItem() {
            patchState(store, { newItem: null} );
        },

        activate: rxMethod<string>(
            pipe(
                tap(itemId => patchState(store, { isLoading: true })),
                switchMap((itemId) => 
                    wordService.activateSearchWord(itemId).pipe(
                        tapResponse({
                            next: () => {
                                const activeCode = store.filter()?.active ?? ActiveEnumOptions.All
                                if(activeCode === ActiveEnumOptions.All || activeCode === ActiveEnumOptions.Inactive) {
                                    patchState(store, updateEntity({ id: itemId, changes: { isActive: true } }));
                                } 
                                else {
                                    patchState(store, removeEntity(itemId));
                                }
                            },
                            error: console.error,
                            finalize: () => patchState(store, { isLoading: false })
                        })
                    ),  
                )
            )  
        ),

        deactivate: rxMethod<string>(
            pipe(
                tap(itemId => patchState(store, { isLoading: true })),
                switchMap((itemId) => 
                wordService.deactivateSearchWord(itemId).pipe(
                        tapResponse({
                            next: () => {
                                const activeCode = store.filter()?.active ?? ActiveEnumOptions.All;
                                if(activeCode === ActiveEnumOptions.All || activeCode === ActiveEnumOptions.Active) {
                                    patchState(store, updateEntity({ id: itemId, changes: { isActive: false } }));
                                } 
                                else {
                                    patchState(store, removeEntity(itemId));
                                }
                            },
                            error: console.error,
                            finalize: () => patchState(store, { isLoading: false })
                        })
                    ),  
                )
            )  
        ),

        activateEditMode: rxMethod<SearchWordItem | null>(
            pipe(
                tap((item) => {
                    modalService.open(store.searchWordModal());
                    patchState(store, { updateItem: item, isLoading: true });
                }),
                filter(item => item != null),
                switchMap((item) => 
                    wordService.getSearchWordSyncStatistic(item!.id).pipe(
                        map(mapToSyncStatsViewModel),
                        tapResponse({
                            next: (res) => { patchState(store, {searchWordModalSyncStats: res})},
                            error: console.error,
                            finalize: () => patchState(store, { isLoading: false })
                        })
                    ),  
                )
            )
        ),

        deactivateEditMode() {
            patchState(store, { updateItem: null, searchWordModalSyncStats: null });
            modalService.close(store.searchWordModal());
        }
    }))
);