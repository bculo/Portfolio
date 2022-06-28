import { createAction, props } from "@ngrx/store";
import { CreateSetting, Setting } from "./settings.models";

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

const SETTING_ADD = "[Settings] Add start";
const SETTING_ADD_SUCCESS = "[Settings] Add success";
const SETTING_ADD_ERROR = "[Settings] Add error";

export const addSetting = createAction(
    SETTING_ADD, 
    props<{
        setting: CreateSetting
    }>()
);

export const addSettingSuccess = createAction(
    SETTING_ADD_SUCCESS,
    props<{
        setting: Setting
    }>()
);

export const addSettingError = createAction(
    SETTING_ADD_ERROR,
    props<{
        error: string
    }>()
);