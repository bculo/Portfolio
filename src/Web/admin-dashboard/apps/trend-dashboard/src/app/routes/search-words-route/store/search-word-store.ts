import {
    signalStore,
    withMethods,
    withState,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { computed, inject } from '@angular/core';
import { mergeMap, pipe, switchMap, zip } from 'rxjs';
import { tapResponse } from '@ngrx/operators';
import { withDevtools } from '@angular-architects/ngrx-toolkit'
import { SearchWordService } from '../../../shared/services/open-api';

interface SearchWordState {
    isLoading: boolean
}

const initialState: SearchWordState = {
    isLoading: true,
}


export const SearchWordStore = signalStore(
    { providedIn: 'root' },
    withState(initialState),
    withDevtools('search-word'),
    withMethods((store, service = inject(SearchWordService)) => ({
        load: rxMethod<void>(
            switchMap(() => 
                service.filter()
            ),        
        ),
    }))
);