import { createSelector } from "@ngrx/store";
import { getSyncModuleState, SyncAppState } from "..";
import { FetchSyncStatuses, ManageSettings } from "./sync.reducer";

/*
SYNC STATUSES
*/

export const getSyncStatusesState = createSelector(
    getSyncModuleState,
    (state: SyncAppState) => state.sync.fetchStatuses
);

export const syncGetLoading = createSelector(
    getSyncStatusesState,
    (state: FetchSyncStatuses) => state.loading
);

export const syncGetError = createSelector(
    getSyncStatusesState,
    (state: FetchSyncStatuses) => state.error
);

export const syncGetItems = createSelector(
    getSyncStatusesState,
    (state: FetchSyncStatuses) => state.items
);

/*
SETTINGS SECTION
*/

export const getSettingsState = createSelector(
    getSyncModuleState,
    (state: SyncAppState) => state.sync.manageSettings
);

export const settingGetLoading = createSelector(
    getSettingsState,
    (state: ManageSettings) => state.loading
);

export const settingGetError = createSelector(
    getSettingsState,
    (state: ManageSettings) => state.error
);

export const settingGetValidationErrors = createSelector(
    getSettingsState,
    (state: ManageSettings) => state.errors
);

