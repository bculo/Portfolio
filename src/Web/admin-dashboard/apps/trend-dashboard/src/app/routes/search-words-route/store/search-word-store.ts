import {
    patchState,
    signalStore,
    withMethods,
    withState,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { computed, inject } from '@angular/core';
import { map, mergeMap, pipe, switchMap, tap, zip } from 'rxjs';
import { tapResponse } from '@ngrx/operators';
import { withDevtools } from '@angular-architects/ngrx-toolkit'
import { SearchWordService } from '../../../shared/services/open-api';
import { SearchWordFilterModel } from '../models/search-words.model';
import { mapToDto } from '../mappers/mapper';

interface SearchWordState {
    isLoading: boolean
}

const initialState: SearchWordState = {
    isLoading: false,
}


export const SearchWordStore = signalStore(
    { providedIn: 'root' },
    withState(initialState),
    withDevtools('search-word'),
    withMethods((store, service = inject(SearchWordService)) => ({
        load: rxMethod<SearchWordFilterModel>(
            pipe(
                map(mapToDto),
                tap(dto => patchState(store, { isLoading: true })),
                switchMap((dto) => 
                    service.filter(dto).pipe(
                        tapResponse({
                            next: () => patchState(store, { }),
                            error: console.error,
                            finalize: () => patchState(store, { isLoading: false })
                        })
                    ),  
                )
            )  
        ),
    }))
);