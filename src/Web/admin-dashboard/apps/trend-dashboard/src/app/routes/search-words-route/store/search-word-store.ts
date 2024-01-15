import {
    patchState,
    signalStore,
    withComputed,
    withMethods,
    withState,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { computed, inject } from '@angular/core';
import { filter, map, pipe, switchMap, tap, zip } from 'rxjs';
import { tapResponse } from '@ngrx/operators';
import { withDevtools } from '@angular-architects/ngrx-toolkit'
import { SearchWordService } from '../../../shared/services/open-api';
import { SearchWordFilterModel, SearchWordItem, SearchWordStats } from '../models/search-words.model';
import { mapToFilterReqDto, mapToFilterViewModel, mapToSyncStatsViewModel } from '../mappers/mapper';
import { removeEntity, setAllEntities, updateEntity, withEntities } from '@ngrx/signals/entities';
import { ActiveEnumOptions } from '../../../shared/enums/enums';
import { SideModalService } from '../../../shared/components/side-modal/side-modal.service';

interface SearchWordState {
    filterHash: string | null,
    filter: SearchWordFilterModel | null,
    searchWordModal: string,
    searchWordModalSyncStats: SearchWordStats | null,
    updateItem: SearchWordItem | null,
    sideNavigationVisible: boolean,
    isLoading: boolean,
    totalCount: number
}

const initialState: SearchWordState = {
    searchWordModalSyncStats: null,
    sideNavigationVisible: false,
    filterHash: null,
    filter: null,
    searchWordModal: 'search-word-modal',
    isLoading: false,
    totalCount: 0,
    updateItem: null
}


export const SearchWordStore = signalStore(
    { providedIn: 'root' },
    withEntities<SearchWordItem>(),
    withState(initialState),
    withDevtools('search-word'),
    withMethods((store, wordService = inject(SearchWordService), modalService = inject(SideModalService)) => ({

        fetch: rxMethod<SearchWordFilterModel>(
            pipe(
                filter((filter) => JSON.stringify(filter) !== store.filterHash()),
                tap((filter) => patchState(store, {filter: filter, filterHash: JSON.stringify(filter), isLoading: true })),
                map(mapToFilterReqDto),
                switchMap((dto) => 
                wordService.filter(dto).pipe(
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

        deactivate: rxMethod<string>(
            pipe(
                tap(itemId => patchState(store, { isLoading: true })),
                switchMap((itemId) => 
                wordService.deactivate(itemId).pipe(
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
        activate: rxMethod<string>(
            pipe(
                tap(itemId => patchState(store, { isLoading: true })),
                switchMap((itemId) => 
                    wordService.activate(itemId).pipe(
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

        activateEditMode: rxMethod<SearchWordItem | null>(
            pipe(
                tap((item) => {
                    modalService.open(store.searchWordModal());
                    patchState(store, { updateItem: item, sideNavigationVisible: true, isLoading: true });
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
            patchState(store, { updateItem: null, sideNavigationVisible: false, searchWordModalSyncStats: null });
            modalService.close(store.searchWordModal());
        }
    }))
);