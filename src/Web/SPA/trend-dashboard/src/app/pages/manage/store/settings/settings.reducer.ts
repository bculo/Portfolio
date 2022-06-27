import { createEntityAdapter, EntityAdapter, EntityState } from "@ngrx/entity";
import { createReducer, on } from "@ngrx/store";
import { Setting } from "./settings.models";

import * as settingsActions from './settings.actions';

export const adapter: EntityAdapter<Setting> = createEntityAdapter<Setting>();

export interface State extends EntityState<Setting> {
    loading: boolean;
    error: string;
};

const initialState: State = adapter.getInitialState({
    error: null,
    loading: false,
});
   

export const reducer = createReducer(
    initialState,

    on(settingsActions.settingsFetch, (state) => {
        return {
            ...state,
            loading: true
        }
    }),

    on(settingsActions.settingsFetchError, (state, { message }) => {
        return {
            ...state,
            loading: false,
            error: message
        }
    }),

    on(settingsActions.settingsFetchSuccess, (state, { items }) => {
        return adapter.setAll(items, {
            ...state,
            loading: false
        })
    }),
);