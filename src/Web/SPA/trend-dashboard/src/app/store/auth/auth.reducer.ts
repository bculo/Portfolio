import { createReducer, on } from "@ngrx/store";

import * as authActions from './auth.actions';

export interface State {
    isAuthenticated: boolean;
    username: string;
    email: string;
    loading: boolean;
    refreshingToken: boolean;
    error: string;
}

export const initialState: State = {
    isAuthenticated: false,
    error: null,
    loading: false,
    refreshingToken: false,
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