import { createSelector } from "@ngrx/store";
import { syncUserModuleState } from "../index";

export const syncState = createSelector(
    syncUserModuleState,
    (state) => state.sync
);

export const getItemsSync = createSelector(
    syncState,
    (state) => state.items
);

export const getLoadingSync = createSelector(
    syncState,
    (state) => state.loading
);

export const getErrorSync = createSelector(
    syncState,
    (state) => state.error
);
