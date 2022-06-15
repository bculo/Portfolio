import { createAction, props } from "@ngrx/store";
import { AddSyncSetting, DictionaryList, SyncStatus } from "./sync.models";

const FETCH_STATUSES = '[Sync] Fetch: Start';
const FETCH_STATUSES_ERROR = '[Sync] Fetch: Error';
const FETCH_STATUSES_SUCCESS = '[Sync] Fetch: Success';

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

const ADD_NEW_WORD = '[Sync] Add: Start';
const ADD_NEW_WORD_ERROR = '[Sync] Add: Error';
const ADD_NEW_WORD_VALIDATION_ERROR = '[Sync] Add: Error Validation';
const ADD_NEW_WORD_SUCCESS = '[Sync] Add: Success';

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