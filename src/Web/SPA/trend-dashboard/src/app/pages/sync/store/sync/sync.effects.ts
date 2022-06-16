import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, of, switchMap, tap } from "rxjs";

import * as SyncAction from './sync.actions'; 
import { AddSyncSetting, SyncStatus } from "./index";
import { isValidationException } from "src/app/shared/utils/error-response";
import { FormHelperService } from "src/app/services/form-mapper/form-helper.service";
import { SETTINGS_FORM_IDENTIFIER } from "../../constants";

@Injectable()
export class SyncEffects {

    constructor(private actions$: Actions, private http: HttpClient, private formHelper: FormHelperService){}

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

    addNewSetting$ = createEffect(() => this.actions$.pipe(
        ofType(SyncAction.addNewWord),
        map((action) => action.newSetting),
        switchMap((request: AddSyncSetting) => {
            return this.http.post<AddSyncSetting>("http://localhost:5276/api/Sync/AddNewSearchWord", request).pipe(
                tap((response) => console.log(response)),
                map(() => SyncAction.addNewWordSuccess()),
                catchError(error => {
                    if(!isValidationException(error)) 
                        return of(SyncAction.addNewWordError({error: error.message}));
                    this.formHelper.handleValidationError(SETTINGS_FORM_IDENTIFIER, error.error.errors);
                    return of(SyncAction.addNewWordValidationError({ errors: error.error.errors }));
                })
            )
        })
    ))

}