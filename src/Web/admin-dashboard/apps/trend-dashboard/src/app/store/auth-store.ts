import {
    signalStore,
    patchState,
    withMethods,
    withState,
    withHooks,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { inject } from '@angular/core';
import { pipe, switchMap, tap } from 'rxjs';
import { tapResponse } from '@ngrx/operators';
import { KeycloakService } from '../shared/services/keycloak.service';
import { withDevtools } from '@angular-architects/ngrx-toolkit'

interface AuthState {
    isLoading: boolean
    isAuthenticated: boolean
    userName: string | null,
    isAdmin: boolean,
    authToken: string | null,
    refreshToken: string | null,
    idToken: string | null
}

const initialState: AuthState = {
    isLoading: true,
    isAuthenticated: false,
    authToken: null,
    idToken: null,
    refreshToken: null,
    isAdmin: false,
    userName: null
}

export const AuthStore = signalStore(
    { providedIn: 'root' },
    withState(initialState),
    withDevtools('auth'),
    withMethods((store, keycloak = inject(KeycloakService)) => ({
        init: rxMethod<void>(
            pipe(
                switchMap(() =>
                    keycloak.configure().pipe(
                        tapResponse({
                            next: (response) => patchState(store, { 
                                isAuthenticated: response.isAuthenticated,
                                idToken: response.userInfo?.idToken,
                                refreshToken: response.userInfo?.refreshToken,
                                authToken: response.userInfo?.token,
                                isAdmin: response.userInfo?.isAdmin ?? false,
                                userName: response.userInfo?.userName
                            }),
                            error: console.error,
                            finalize: () => patchState(store, { isLoading: false }),
                        }),
                    )
                ),
            ),
        ),
    })),
    withHooks({
        onInit({ init }) {
            init();
        },
    }),
);