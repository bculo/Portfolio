import { createAction, props } from "@ngrx/store";
import { ControlItem } from "./dictionaries.models";

const FETCH_DICTIONARIES = '[Sync] Fetch dictionaries: start';
const FETCH_DICTIONARIES_SUCCESS = '[Sync] Fetch dictionaries: success';
const FETCH_DICTIONARIES_ERROR = '[Sync] Fetch dictionaries: error';

export const fetchDictionaries = createAction(
    FETCH_DICTIONARIES
);

export const fetchDictionariesSuccess = createAction(
    FETCH_DICTIONARIES_SUCCESS,
    props<{
        engines: ControlItem[],
        types: ControlItem[],
    }>()
);

export const fetchDictionariesError = createAction(
    FETCH_DICTIONARIES_ERROR,
    props<{
        error: string
    }>()
);