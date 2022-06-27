import { createAction, props } from "@ngrx/store";
import { Setting } from "./settings.models";

const SETTINGS_FETCH = "[Settings] Fetch start";
const SETTINGS_FETCH_SUCCESS = "[Settings] Fetch success";
const SETTINGS_FETCH_ERROR = "[Settings] Fetch error";

export const settingsFetch = createAction(
    SETTINGS_FETCH
);

export const settingsFetchSuccess = createAction(
    SETTINGS_FETCH_SUCCESS,
    props<{
        items: Setting[]
    }>()
);

export const settingsFetchError = createAction(
    SETTINGS_FETCH_ERROR,
    props<{
        message: string
    }>()
);