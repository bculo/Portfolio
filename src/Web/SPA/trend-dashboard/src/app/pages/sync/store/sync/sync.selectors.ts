import { createSelector } from "@ngrx/store";
import { getSyncModuleState, SyncAppState } from "..";
import { SyncState } from "./sync.reducer";

export const getSyncState = createSelector(
    getSyncModuleState,
    (state: SyncAppState) => state.sync
);

export const getLoading = createSelector(
    getSyncState,
    (state: SyncState) => state.loading
);

export const getError = createSelector(
    getSyncState,
    (state: SyncState) => state.error
);

export const getItems = createSelector(
    getSyncState,
    (state: SyncState) => state.items
);