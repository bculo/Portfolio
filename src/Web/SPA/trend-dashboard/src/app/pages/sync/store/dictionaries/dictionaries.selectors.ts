import { createSelector } from "@ngrx/store";
import { syncUserModuleState } from "../index";

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
