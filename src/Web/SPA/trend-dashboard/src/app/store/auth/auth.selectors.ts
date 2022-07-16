import { createFeatureSelector, createSelector } from "@ngrx/store";
import { State } from "./auth.reducer";

export const getAuthState = createFeatureSelector<State>('auth');

export const isAuthenticated = createSelector(
    getAuthState,
    (state) => state.isAuthenticated
);

export const isAdmin = createSelector(
    getAuthState,
    (state) => state.role == "Admin"
);