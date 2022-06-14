import { createAction, props } from "@ngrx/store";
import { SyncStatus } from "./sync.models";

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