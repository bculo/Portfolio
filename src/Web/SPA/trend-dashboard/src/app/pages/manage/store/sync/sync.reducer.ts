import { createReducer, on } from '@ngrx/store';
import * as SyncActions from './sync.actions';
import { SyncStatus } from './sync.models';

import { EntityState, EntityAdapter, createEntityAdapter } from '@ngrx/entity';

export const adapter: EntityAdapter<SyncStatus> = createEntityAdapter<SyncStatus>();

export interface State extends EntityState<SyncStatus> {
    selectedItem: SyncStatus,
    executingSync: boolean;
    loading: boolean;
    error: string;
};

const initialState: State = adapter.getInitialState({
    loading: null,
    executingSync: null,
    error: null,
    selectedItem: null
});

export const reducer = createReducer(
    initialState,

    //
    //FETCH COLLECTION
    //

    on(SyncActions.fetchStatuses, (state) => {
        return {
            ...state,
            loading: true
        }
    }),

    on(SyncActions.fetchStatusesSuccess, (state, { items }) => {
        return adapter.addMany(items, {
            ...state,
            loading: false
        });
    }),

    on(SyncActions.fetchStatusesError, (state, { error }) => {
        return {
            ...state,
            loading: false,
            error: error
        }
    }),

    //
    //SYNC
    //

    on(SyncActions.sync, (state) => {
        return {
            ...state,
            executingSync: true
        }
    }),

    on(SyncActions.syncSuccess, (state) => {
        return {
            ...state,
            executingSync: false,
        }
    }),

    on(SyncActions.syncError, (state, { error }) => {
        return {
            ...state,
            executingSync: false,
            error: error
        }
    }),

    ///
    ///FETCH ITEM
    ///
    on(SyncActions.fetchSyncItem, (state) => {
        return {
            ...state,
            loading: true,
        }
    }),

    on(SyncActions.fetchSyncItemSuccess, (state, { status }) => {
        return {
            ...state,
            loading: false,
            selectedItem: status
        }
    }),

    on(SyncActions.fetchSyncItemError, (state, { error }) => {
        return {
            ...state,
            loading: false,
            error: error
        }
    }),
);

