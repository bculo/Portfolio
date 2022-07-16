import { createReducer, on } from "@ngrx/store";

import * as authActions from './auth.actions';
import { UserType } from "./auth.models";

export interface State {
    isAuthenticated: boolean;
    username: string;
    email: string;
    loading: boolean;
    refreshingToken: boolean;
    error: string;
    role: UserType
}

export const initialState: State = {
    isAuthenticated: false,
    error: null,
    loading: false,
    refreshingToken: false,
    username: null,
    email: null,
    role: null
}

export const authReducer = createReducer(
    initialState,

    on(authActions.userAuthenticationStarted, (state) => {
        return {
            ...state,
            loading: true,
        }
    }),

    on(authActions.userAuthenticationFailed, (state) => {
        return {
            ...state,
            loading: false,
        }
    }),


    on(authActions.userAuthenticated, (state, {status}) => {
        return {
            ...state,
            email: status.email,
            username: status.username,
            isAuthenticated: true,
            role: status.role,
            loading: false,
        }
    }),

    on(authActions.userLogout, (state) => {
        return {
            ...state,
        }
    }),
)