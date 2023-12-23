import { createReducer, on } from '@ngrx/store';
import * as SyncActions from './sync.actions';
import { SyncStatus } from './sync.models';

import { EntityState, EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import { PaginationState } from 'src/app/models/frontend/page';

export const adapter: EntityAdapter<SyncStatus> = createEntityAdapter<SyncStatus>();

export interface State extends EntityState<SyncStatus> {
    pageState: PaginationState
    initializationFailed: boolean;
    selectedItem: SyncStatus,
    executingSync: boolean;
    loading: boolean;
    error: string;
};

const initialState: State = adapter.getInitialState({
    initializationFailed: null,

    pageState: {
        page: 0,
        take: 5,
        totalItems: 0
    },

    loading: null, //loading items?

    executingSync: null, //is sync button pressed?

    selectedItem: null, //helper for selected item (show details)

    error: null, //error message
});

export const reducer = createReducer(
    initialState,

    //
    //SET PAGE TAKE LIMIT
    //

    on(SyncActions.setPageTakeLimit, (state, { take }) => {
        return {
            ...state,
            pageState: {
                ...state.pageState,
                take: (take > 0) ? take : 5
            }
        }
    }),

    //
    //FETCH COLLECTION
    //

    on(SyncActions.fetchStatuses, (state) => {
        return {
            ...state,
            loading: true
        }
    }),

    on(SyncActions.fetchStatusesSuccess, (state, { page }) => {
        return adapter.addMany(page.items, {
            ...state,
            loading: false,
            initializationFailed: false,
            pageState: {
                ...state.pageState,
                page: state.pageState.page + 1,
                totalItems: page.count
            }
        });
    }),

    on(SyncActions.fetchStatusesError, (state, { error }) => {
        return {
            ...state,
            loading: false,
            error: error,
            initializationFailed: true,
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
            executingSync: false
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
            selectedItem: status,
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

