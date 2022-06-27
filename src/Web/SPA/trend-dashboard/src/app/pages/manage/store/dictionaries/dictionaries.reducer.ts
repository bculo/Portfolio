import { createReducer, on } from "@ngrx/store";
import { ControlItem } from "./dictionaries.models";

import * as DictionariesActions from './dictionaries.actions';

export interface State {
    loading: boolean;
    loaded: boolean;
    error: string;
    engines: ControlItem[];
    types: ControlItem[];
};

const initialState: State = {
    engines: [],
    types: [],
    error: null,
    loading: false,
    loaded: false,
};

export const reducer = createReducer(
    initialState,

    on(DictionariesActions.fetchDictionaries, (state) => ({
        ...state,
        loading: true
    })),

    on(DictionariesActions.fetchDictionariesSuccess, (state, { engines, types }) => ({
        ...state,
        loading: false,
        engines: engines,
        types: types,
        loaded: true
    })),

    on(DictionariesActions.fetchDictionariesError, (state, { error }) => ({
        ...state,
        loading: false,
        error: error
    })),
);