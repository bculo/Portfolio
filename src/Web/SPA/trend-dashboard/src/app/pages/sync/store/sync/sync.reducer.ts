import { createReducer, on } from '@ngrx/store';
import * as SyncActions from './sync.actions';
import { DictionaryList, SyncStatus } from './sync.models';

export interface FetchSyncStatuses {
    loading: boolean;
    error: string;
    items: SyncStatus[]
}

export interface ManageSettings {
    loading: boolean;
    error: string;
    errors: DictionaryList<string>;
    validationException: boolean;
}

export interface SyncState {
    fetchStatuses: FetchSyncStatuses
    manageSettings: ManageSettings
}

const initialState: SyncState = {
    fetchStatuses: {
        error: null,
        items: null,
        loading: false,
    },
    manageSettings: {
        error: null,
        errors: null,
        loading: false,
        validationException: false
    }
}

export const syncReducer = createReducer(
    initialState,

    on(SyncActions.fetchStatuses, (state) => {
        return {
            ...state,
            fetchStatuses: {
                ...state.fetchStatuses,
                loading: true
            }
        }
    }),

    on(SyncActions.fetchStatusesSuccess, (state, { items }) => {
        return {
            ...state,
            fetchStatuses: {
                ...state.fetchStatuses,
                loading: false,
                items: items
            }
        }
    }),

    on(SyncActions.fetchStatusesError, (state, { error }) => {
        return {
            ...state,
            fetchStatuses: {
                ...state.fetchStatuses,
                loading: false,
                error: error
            }
        }
    }),

    on(SyncActions.addNewWord, (state) => {
        return {
            ...state,
            manageSettings: {
                ...state.manageSettings,
                loading: true,
            }
        }
    }),

    on(SyncActions.addNewWordError, (state, { error }) => {
        return {
            ...state,
            manageSettings: {
                ...state.manageSettings,
                loading: false,
                error: error
            }
        }
    }),

    on(SyncActions.addNewWordValidationError, (state, { errors }) => {
        return {
            ...state,
            manageSettings: {
                ...state.manageSettings,
                loading: false,
                errors: errors
            }
        }
    }),

    on(SyncActions.addNewWordSuccess, (state) => {
        return {
            ...state,
            manageSettings: {
                ...state.manageSettings,
                loading: false,
            }
        }
    }),

);