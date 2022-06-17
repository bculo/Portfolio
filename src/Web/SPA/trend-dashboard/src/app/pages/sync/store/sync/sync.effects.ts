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

    /*
    addNewSetting$ = createEffect(() => this.actions$.pipe(
        ofType(SyncAction.addNewWord),
        map((action) => action.newSetting),
        switchMap((request: AddSyncSetting) => {
            return this.http.post<AddSyncSetting>("http://localhost:5276/api/Sync/AddNewSearchWord", request).pipe(
                map(() => SyncAction.addNewWordSuccess()),
                catchError(error => {
                    if(!isValidationException(error)) 
                        return of(SyncAction.addNewWordError({error: error.message}));
                    this.formHelper.handleValidationError(SETTINGS_FORM_IDENTIFIER, error.error.errors);
                    return of(SyncAction.addNewWordValidationError({ errors: error.error.errors }));
                })
            )
        })
    ));
    */
}