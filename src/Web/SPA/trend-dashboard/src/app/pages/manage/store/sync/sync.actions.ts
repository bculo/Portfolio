import { createAction, props } from "@ngrx/store";
import { SyncStatus } from "./sync.models";

const FETCH_STATUSES = '[Sync] Fetch colleciton: start';
const FETCH_STATUSES_ERROR = '[Sync] Fetch colleciton: error';
const FETCH_STATUSES_SUCCESS = '[Sync] Fetch colleciton: success';

export const fetchStatuses = createAction(
    FETCH_STATUSES,
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

const SYNC = '[Sync]: Execute: start';
const SYNC_ERROR = '[Sync]: Execute: error';
const SYNC_SUCCESS = '[Sync]: Execute: success';

export const sync = createAction(
    SYNC
);

export const syncError = createAction(
    SYNC_ERROR,
    props<{
        error: string
    }>()
);

export const syncSuccess = createAction(
    SYNC_SUCCESS
);

const FETCH_SYNC_ITEM = "[Sync] Fetch item: start";
const FETCH_SYNC_ITEM_ERROR = "[Sync] Fetch item: error";
const FETCH_SYNC_ITEM_SUCCES = "[Sync] Fetch item: success";

export const fetchSyncItem = createAction(
    FETCH_SYNC_ITEM,
    props<{
        id: string
    }>()
);

export const fetchSyncItemError = createAction(
    FETCH_SYNC_ITEM_ERROR,
    props<{
        error: string
    }>()
);

export const fetchSyncItemSuccess = createAction(
    FETCH_SYNC_ITEM_SUCCES,
    props<{
        status: SyncStatus
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

