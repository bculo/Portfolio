import { createReducer, on } from "@ngrx/store";

import * as authActions from './auth.actions';

export interface State {
    isAuthenticated: boolean;
    refreshToken: string;
    token: string;
    idToken: string;
    username: string;
    email: string;
    loading: boolean;
    refreshingToken: boolean;
    error: string;
}

export const initialState: State = {
    isAuthenticated: false,
    error: null,
    idToken: null,
    loading: false,
    refreshingToken: false,
    refreshToken: null,
    token: null,
    username: null,
    email: null
}

export const authReducer = createReducer(
    initialState,

    on(authActions.userAuthenticated, (state, {status}) => {
        return {
            ...state,
            email: status.email,
            username: status.username,
            isAuthenticated: true
        }
    }),

    on(authActions.userLogout, (state) => {
        return {
            ...state,
        }
    }),
)