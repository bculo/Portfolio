import {
    signalStore,
    patchState,
    withMethods,
    withState,
    withHooks,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { inject } from '@angular/core';
import { EMPTY, debounceTime, distinctUntilChanged, filter, iif, mergeMap, of, pipe, switchMap, take, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { tapResponse } from '@ngrx/operators';
import { KeycloakService } from '../shared/services/keycloak.service';

interface AuthState {
    isLoading: boolean
    isAuthenticated: boolean
}

const initialState: AuthState = {
    isLoading: true,
    isAuthenticated: false
}

export const AuthStore = signalStore(
    { providedIn: 'root' },
    withState(initialState),
    withMethods((store) => ({
        setLoading(value: boolean): void {
            patchState(store, { isLoading: value });
        },
        setAuth(value: boolean): void {
            patchState(store, { isAuthenticated: value });
        }
    })),
    withHooks({
        onInit() {
            console.log("INVOKED")
        },
    }),
);