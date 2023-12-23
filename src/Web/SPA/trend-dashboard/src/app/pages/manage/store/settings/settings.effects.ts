import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, of, switchMap, tap } from "rxjs";
import { FormHelperService } from "src/app/services/form-mapper/form-helper.service";
import { NotificationService } from "src/app/services/notification/notification.service";
import { getMessageFromBadRequest, handleServerError, isValidationException } from "src/app/shared/utils/error-response";
import { SETTINGS_FORM_IDENTIFIER } from "../../constants";

import * as settingsActions from './settings.actions';
import * as settingsModels from './settings.models';

import { environment } from "src/environments/environment";

@Injectable()
export class SettingsEffects {

    constructor(private actions$: Actions, 
        private http: HttpClient, 
        private notification: NotificationService,
        private helper: FormHelperService) { }

    loadSettings$ = createEffect(() => this.actions$.pipe(
        ofType(settingsActions.settingsFetch),
        switchMap(() => {
            return this.http.get<settingsModels.Setting[]>(`${environment.trendApiBaseUrl}/SearchWord/GetSearchWords`).pipe(
                map((response) => settingsActions.settingsFetchSuccess({items: response})),
                catchError((error) => {
                    return of(settingsActions.settingsFetchError({message: handleServerError(error, this.notification)}));
                })
            )
        })
    ));

    addSettings = createEffect(() => this.actions$.pipe(
        ofType(settingsActions.addSetting),
        map(action => action.setting),
        switchMap((request: settingsModels.CreateSetting) => {
            return this.http.post<settingsModels.Setting>(`${environment.trendApiBaseUrl}/SearchWord/AddNewSearchWord`, request).pipe(
                map((response) => settingsActions.addSettingSuccess({ setting: response })),
                tap(() => this.notification.success("Item successfuly added to collection")),
                catchError((error) => {
                    return of(settingsActions.addSettingError({ error: handleServerError(error, this.notification, this.helper, SETTINGS_FORM_IDENTIFIER) }));
                })
            )
        })
    ));
}