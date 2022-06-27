import { createFeatureSelector, createSelector } from "@ngrx/store";
import { SyncModuleState } from "..";
import { SYNC_MODULE_STATE } from "../../constants";
import { adapter } from "./settings.reducer";

export const syncUserModuleState = createFeatureSelector<SyncModuleState>(SYNC_MODULE_STATE);

export const settingsState = createSelector(
    syncUserModuleState,
    (state) => state.settings
);

export const {
    selectIds,
    selectEntities,
    selectAll,
    selectTotal,
} = adapter.getSelectors(settingsState);

export const loading = createSelector(
    settingsState,
    (state) => state.loading
)