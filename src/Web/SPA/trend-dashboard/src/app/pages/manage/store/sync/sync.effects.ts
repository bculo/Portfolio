import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, of, switchMap } from "rxjs";

import * as SyncAction from './sync.actions'; 
import { FormHelperService } from "src/app/services/form-mapper/form-helper.service";
import { SyncStatus } from "./sync.models";

@Injectable()
export class SyncEffects {

    constructor(private actions$: Actions, private http: HttpClient, private formHelper: FormHelperService){}

    loadStatuses$ = createEffect(() => this.actions$.pipe(
        ofType(SyncAction.fetchStatuses),
        switchMap(() => {
            return this.http.get<SyncStatus[]>("http://localhost:5276/api/Sync/GetSyncStatuses").pipe(
                map(response => SyncAction.fetchStatusesSuccess({items: response})),
                catchError(error => of(SyncAction.fetchStatusesError({error: error.message})))
            )
        })
    ));

    fetchSyncItem$ = createEffect(() => this.actions$.pipe(
        ofType(SyncAction.fetchSyncItem),
        map((action) => action.id),
        switchMap((id: string) => {
            return this.http.get<SyncStatus>(`http://localhost:5276/api/Sync/GetSync/${id}`).pipe(
                map(response => SyncAction.fetchSyncItemSuccess({status: response})),
                catchError(error => of(SyncAction.fetchStatusesError({error: error.message})))
            )
        })
    ));

    executeSync$ = createEffect(() => this.actions$.pipe(
        ofType(SyncAction.sync),
        switchMap(() => {
            return this.http.get("http://localhost:5276/api/Sync/Sync").pipe(
                map(() => SyncAction.syncSuccess()),
                catchError(error => of(SyncAction.syncError({error: error.message})))
            )
        })
    ))
}