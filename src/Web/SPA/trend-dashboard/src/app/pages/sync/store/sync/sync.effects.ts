import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, of, switchMap, tap } from "rxjs";

import * as SyncAction from './sync.actions'; 
import { SyncStatus } from "./index";

@Injectable()
export class SyncEffects {

    constructor(private actions$: Actions, private http: HttpClient){}

    loadStatuses$ = createEffect(() => this.actions$.pipe(
        ofType(SyncAction.fetchStatuses),
        switchMap(() => {
            return this.http.get<SyncStatus[]>("http://localhost:5276/api/Sync/GetSyncStatuses").pipe(
                tap(response => console.log(response)),
                map(response => SyncAction.fetchStatusesSuccess({items: response})),
                catchError(error => of(SyncAction.fetchStatusesError({error: error.message})))
            )
        })
    ));

}