import { state } from "@angular/animations";
import { createFeatureSelector, createSelector } from "@ngrx/store";
import { SyncModuleState } from "..";
import { SYNC_MODULE_STATE } from "../../constants";

export const syncUserModuleState = createFeatureSelector<SyncModuleState>(SYNC_MODULE_STATE);

export const dictState = createSelector(
    syncUserModuleState,
    (state) => state.dictionaries
);

export const getContextTypesDict = createSelector(
    dictState,
    (state) => state.types
);

export const getEngineTypesDict = createSelector(
    dictState,
    (state) => state.engines
);

export const getLoadingDict = createSelector(
    dictState,
    (state) => state.loading
);

export const getErrorDict = createSelector(
    dictState,
    (state) => state.error
);

export const areLoaded = createSelector(
    dictState,
    (state) => state.loaded
)

export const shouldLoad = createSelector(
    dictState,
    (state) => {
        if(!state.loaded && !state.loading && !state.error)
            return true;
        return false;
    }
)
