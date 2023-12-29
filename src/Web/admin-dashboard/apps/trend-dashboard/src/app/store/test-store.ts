import {
    signalStore,
    patchState,
    withMethods,
    withState,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { inject } from '@angular/core';
import { EMPTY, debounceTime, distinctUntilChanged, filter, iif, mergeMap, of, pipe, switchMap, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { tapResponse } from '@ngrx/operators';

interface CounterState {
    isLoading: boolean
    loaded: boolean
    count: number
}

const initialState: CounterState = {
    count: 0,
    isLoading: false,
    loaded: true
}

export const CounterStore = signalStore(
    { providedIn: 'root' },
    withState(initialState),
    withMethods((store, client = inject(HttpClient)) => ({
      load: rxMethod<void>(
        pipe(
          filter(() => !store.loaded()),
          tap(() => patchState(store, { isLoading: true })),
          switchMap(() =>
            client.get<unknown>("http://localhost:5276/api/v1/News/GetLatestNews").pipe(
              tapResponse({
                next: (something) => console.log(something),
                error: console.error,
                finalize: () => patchState(store, { isLoading: false }),
              }),
            ),
          ),
        ),
      ),
    })),
);