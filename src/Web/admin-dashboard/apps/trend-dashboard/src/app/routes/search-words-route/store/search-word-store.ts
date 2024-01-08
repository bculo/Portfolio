import {
    patchState,
    signalStore,
    withMethods,
    withState,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { inject } from '@angular/core';
import { map, pipe, switchMap, tap, zip } from 'rxjs';
import { tapResponse } from '@ngrx/operators';
import { withDevtools } from '@angular-architects/ngrx-toolkit'
import { SearchWordService } from '../../../shared/services/open-api';
import { SearchWordFilterModel, SearchWordItem } from '../models/search-words.model';
import { mapToFilterReqDto, mapToFilterViewModel } from '../mappers/mapper';
import { setAllEntities, withEntities } from '@ngrx/signals/entities';

interface SearchWordState {
    isLoading: boolean,
    totalCount: number
}

const initialState: SearchWordState = {
    isLoading: false,
    totalCount: 0
}


export const SearchWordStore = signalStore(
    { providedIn: 'root' },
    withEntities<SearchWordItem>(),
    withState(initialState),
    withDevtools('search-word'),
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
    }))
);