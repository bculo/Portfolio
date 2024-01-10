import {
    patchState,
    signalStore,
    withComputed,
    withMethods,
    withState,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { computed, inject } from '@angular/core';
import { map, pipe, switchMap, tap, zip } from 'rxjs';
import { tapResponse } from '@ngrx/operators';
import { withDevtools } from '@angular-architects/ngrx-toolkit'
import { SearchWordService } from '../../../shared/services/open-api';
import { SearchWordFilterModel, SearchWordItem } from '../models/search-words.model';
import { mapToFilterReqDto, mapToFilterViewModel } from '../mappers/mapper';
import { setAllEntities, updateEntity, withEntities } from '@ngrx/signals/entities';

interface SearchWordState {
    updateItem: SearchWordItem | null,
    isLoading: boolean,
    totalCount: number
}

const initialState: SearchWordState = {
    isLoading: false,
    totalCount: 0,
    updateItem: null
}


export const SearchWordStore = signalStore(
    { providedIn: 'root' },
    withEntities<SearchWordItem>(),
    withState(initialState),
    withDevtools('search-word'),
    withComputed(({ updateItem }) => ({
        isUpdateMode: computed(() => updateItem),
    })),
    withMethods((store, service = inject(SearchWordService)) => ({

        initialFetch: rxMethod<SearchWordFilterModel>(
            pipe(
                map(mapToFilterReqDto),
                tap(dto => patchState(store, { isLoading: true })),
                switchMap((dto) => 
                    service.filter(dto).pipe(
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
                    service.deactivate(itemId).pipe(
                        tapResponse({
                            next: () => patchState(store, updateEntity({ id: itemId, changes: { isActive: false } })),
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
                    service.activate(itemId).pipe(
                        tapResponse({
                            next: () => patchState(store, updateEntity({ id: itemId, changes: { isActive: true } })),
                            error: console.error,
                            finalize: () => patchState(store, { isLoading: false })
                        })
                    ),  
                )
            )  
        ),

        activateUpdateMode(item: SearchWordItem) {
            patchState(store, { updateItem: item });
        },

        deactivateUpdateMode() {
            patchState(store, { updateItem: null });
        }
    }))
);