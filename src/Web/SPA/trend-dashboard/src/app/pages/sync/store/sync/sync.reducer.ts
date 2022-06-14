import { createReducer, on } from '@ngrx/store';
import * as SyncActions from './sync.actions';
import { SyncStatus } from './sync.models';

export interface SyncState {
    loading: boolean;
    error: string;
    items: SyncStatus[]
}

const initialState: SyncState = {
    error: null,
    items: null,
    loading: false
}

export const syncReducer = createReducer(
    initialState,

    on(SyncActions.fetchStatuses, (state) => {
        return {
            ...state,
            loading: true,
        }
    }),

    on(SyncActions.fetchStatusesSuccess, (state, { items }) => {
        return {
            ...state,
            loading: false,
            items: items
        }
    }),

    on(SyncActions.fetchStatusesError, (state, { error }) => {
        return {
            ...state,
            loading: false,
            error: error
        }
    }),

);