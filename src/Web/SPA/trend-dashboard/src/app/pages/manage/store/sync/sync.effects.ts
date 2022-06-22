import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, of, switchMap, withLatestFrom} from "rxjs";

import { PaginatedResult, SyncStatus } from "./sync.models";
import { NotificationService } from "src/app/services/notification/notification.service";
import { Store } from "@ngrx/store";

import * as fromRoot from 'src/app/store/index';

import * as syncAction from './sync.actions';
import * as syncSelectors from './sync.selectors';

@Injectable()
export class SyncEffects {

    constructor(private actions$: Actions, 
        private http: HttpClient, 
        private notificationService: NotificationService,
        private store: Store<fromRoot.State>){}

    loadStatuses$ = createEffect(() => this.actions$.pipe(
        ofType(syncAction.fetchStatuses),
        withLatestFrom(this.store.select(syncSelectors.getNextPageSync)),
        map(([action, page]) => page),
        switchMap((pageRequest) => {       
            return this.http.post<PaginatedResult<SyncStatus>>("http://localhost:5276/api/Sync/GetSyncStatusesPage", pageRequest).pipe(
                map(response => syncAction.fetchStatusesSuccess({ page: response })),
                catchError(error => {
                    this.notificationService.error(error.message);
                    return of(syncAction.fetchStatusesError({error: error.message}))
                })
            )
        })
    ));

    fetchSyncItem$ = createEffect(() => this.actions$.pipe(
        ofType(syncAction.fetchSyncItem),
        map((action) => action.id),
        switchMap((id: string) => {
            return this.http.get<SyncStatus>(`http://localhost:5276/api/Sync/GetSync/${id}`).pipe(
                map(response => syncAction.fetchSyncItemSuccess({status: response})),
                catchError(error => { 
                    this.notificationService.error(error.message);
                    return of(syncAction.fetchStatusesError({error: error.message}))
                })
            )
        })
    ));

    executeSync$ = createEffect(() => this.actions$.pipe(
        ofType(syncAction.sync),
        switchMap(() => {
            return this.http.get("http://localhost:5276/api/Sync/Sync").pipe(
                map(() => syncAction.syncSuccess()),
                catchError(error => { 
                    this.notificationService.error(error.message);
                    return of(syncAction.syncError({error: error.message}))
                })
            )
        })
    ))
}