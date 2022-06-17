import { createAction, props } from "@ngrx/store";
import { SyncStatus } from "./sync.models";

const FETCH_STATUSES = '[Sync] Fetch: start';
const FETCH_STATUSES_ERROR = '[Sync] Fetch: error';
const FETCH_STATUSES_SUCCESS = '[Sync] Fetch: success';

export const fetchStatuses = createAction(
    FETCH_STATUSES
);

export const fetchStatusesError = createAction(
    FETCH_STATUSES_ERROR,
    props<{
        error: string
    }>()
);

export const fetchStatusesSuccess = createAction(
    FETCH_STATUSES_SUCCESS,
    props<{
        items: SyncStatus[]
    }>()
);

/*
const ADD_NEW_WORD = '[Sync] New word: add';
const ADD_NEW_WORD_ERROR = '[Sync] New word: error';
const ADD_NEW_WORD_VALIDATION_ERROR = '[Sync] New word: validation error';
const ADD_NEW_WORD_SUCCESS = '[Sync] New word: success';

export const addNewWord =  createAction(
    ADD_NEW_WORD,
    props<{
        newSetting: AddSyncSetting
    }>()
);

export const addNewWordError =  createAction(
    ADD_NEW_WORD_ERROR,
    props<{
        error: string
    }>()
);

export const addNewWordValidationError =  createAction(
    ADD_NEW_WORD_VALIDATION_ERROR,
    props<{
        errors: DictionaryList<string>
    }>()
);

export const addNewWordSuccess =  createAction(
    ADD_NEW_WORD_SUCCESS,
);
*/

