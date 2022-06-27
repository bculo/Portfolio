import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, of, switchMap } from "rxjs";
import { NotificationService } from "src/app/services/notification/notification.service";

import * as settingsActions from './settings.actions';
import * as settingsModels from './settings.models';

@Injectable()
export class SettingsEffects {

    constructor(private actions$: Actions, private http: HttpClient, private notification: NotificationService) { }

    loadSettings$ = createEffect(() => this.actions$.pipe(
        ofType(settingsActions.settingsFetch),
        switchMap(() => {
            return this.http.get<settingsModels.Setting[]>("http://localhost:5276/api/SearchWord/GetSearchWords").pipe(
                map((response) => settingsActions.settingsFetchSuccess({items: response})),
                catchError((error) => {
                    this.notification.error(error.message);
                    return of(settingsActions.settingsFetchError({message: error.message}));
                })
            )
        })
    ))

}