import { createAction, props } from "@ngrx/store";
import { PaginatedResult, SyncStatus } from "./sync.models";

const SET_PAGE_TAKE_LIMIT = '[Sync] Set page take limit'

export const setPageTakeLimit = createAction(
    SET_PAGE_TAKE_LIMIT,
    props<{
        take: number
    }>()
);

const FETCH_STATUSES = '[Sync] Fetch colleciton: start';
const FETCH_STATUSES_ERROR = '[Sync] Fetch colleciton: error';
const FETCH_STATUSES_SUCCESS = '[Sync] Fetch colleciton: success';

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
        page: PaginatedResult<SyncStatus>
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
    SYNC_SUCCESS,
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


