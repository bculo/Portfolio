import { createFeatureSelector, createSelector } from "@ngrx/store";
import { SyncModuleState } from "..";
import { SYNC_MODULE_STATE } from "../../constants";
import { sync } from "./sync.actions";
import { adapter } from "./sync.reducer";

export const syncUserModuleState = createFeatureSelector<SyncModuleState>(SYNC_MODULE_STATE);

//ROOT SYNC SELECTOR
export const syncState = createSelector(
    syncUserModuleState,
    (state) => state.sync
);

//ADAPTER SELECTORS
export const {
    selectAll,
    selectEntities,
    selectTotal,
    selectIds,   
} = adapter.getSelectors(syncState);

export const selectEntityById = (props: {id: string}) => 
    createSelector(selectEntities, (entities) => {
        return entities[props.id];
    });

export const getAdditionallyFetchedItem = createSelector(
    syncState,
    (state) => state.selectedItem
);

export const getLoadingSync = createSelector(
    syncState,
    (state) => state.loading
);

export const getExecutingSync = createSelector(
    syncState,
    (state) => state.executingSync
);

export const getPageState = createSelector(
    syncState,
    (state) => state.pageState
)

export const initializationFailed = createSelector(
    syncState,
    (state) => state.initializationFailed
)

export const getNextPageSync = createSelector(
    getPageState,
    (state) => ({page: state.page + 1, take: state.take})
)

export const canLoadNextPageSync = createSelector(
    getPageState,
    selectTotal,
    (pageState, total) => pageState.totalItems > total
)

export const fetchingItemsAvailableSync = createSelector(
    canLoadNextPageSync,
    initializationFailed,
    (canLoadNextPageSync, initializationFailed) => {
        if(initializationFailed || !canLoadNextPageSync) 
            return true;
        return false; 
    }
)


