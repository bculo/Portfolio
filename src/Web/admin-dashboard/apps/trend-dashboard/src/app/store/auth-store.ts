import {
    signalStore,
    patchState,
    withMethods,
    withState,
    withHooks,
    withComputed,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { computed, inject } from '@angular/core';
import { pipe, switchMap, tap } from 'rxjs';
import { tapResponse } from '@ngrx/operators';
import { KeycloakService } from '../shared/services/keycloak.service';
import { withDevtools } from '@angular-architects/ngrx-toolkit'
import { AuthWrapper } from '../shared/models/auth.model';

interface AuthState {
    isLoading: boolean
    isAuthenticated: boolean
    userName: string | null,
    email: string | null,
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
    userName: null,
    email: null,
}

export const AuthStore = signalStore(
    { providedIn: 'root' },
    withState(initialState),
    withDevtools('auth'),
    withComputed(({ userName }) => ({
        userNameFirstChar: computed(() => userName() && userName()!.length > 0 ?  userName()![0].toUpperCase() : 'A'),
    })),
    withMethods((store) => ({
        setLoadingFlag(status: boolean) {
            patchState(store, { isLoading: status })
        },
        set(status: AuthWrapper) {
            patchState(store, {
                isAuthenticated: status.isAuthenticated,
                idToken: status.userInfo?.idToken,
                refreshToken: status.userInfo?.refreshToken,
                authToken: status.userInfo?.token,
                isAdmin: status.userInfo?.isAdmin ?? false,
                userName: status.userInfo?.userName,
                email: status.userInfo?.email,
            })
        }
    })),
    withHooks({

    }),
);

/*

        init: rxMethod<void>(
            switchMap(() =>
                keycloak.configure().pipe(
                    tapResponse({
                        next: (response) => patchState(store, { 
                            isAuthenticated: response.isAuthenticated,
                            idToken: response.userInfo?.idToken,
                            refreshToken: response.userInfo?.refreshToken,
                            authToken: response.userInfo?.token,
                            isAdmin: response.userInfo?.isAdmin ?? false,
                            userName: response.userInfo?.userName,
                            email: response.userInfo?.email
                        }),
                        error: console.error,
                        finalize: () => patchState(store, { isLoading: false }),
                    }),
                )
            ),
        ),
        */