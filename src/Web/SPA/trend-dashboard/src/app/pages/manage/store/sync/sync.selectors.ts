import { createSelector } from "@ngrx/store";
import { syncUserModuleState } from "../index";
import { adapter } from "./sync.reducer";

export const syncState = createSelector(
    syncUserModuleState,
    (state) => state.sync
);

export const {
    selectAll,
    selectEntities,
    selectTotal,
    selectIds
} = adapter.getSelectors(syncState);

export const selectEntityById = (props: {id: string}) => 
    createSelector(selectEntities, (entities) => {
        return entities[props.id];
    });

export const getAdditionallyFetchedItem = createSelector(
    syncState,
    (state) => state.selectedItem
)

export const getLoadingSync = createSelector(
    syncState,
    (state) => state.loading
);

export const getExecutingSync = createSelector(
    syncState,
    (state) => state.executingSync
);

export const getErrorSync = createSelector(
    syncState,
    (state) => state.error
);
