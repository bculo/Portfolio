import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, of, switchMap, zip } from "rxjs";
import { NotificationService } from "src/app/services/notification/notification.service";
import { handleServerError } from "src/app/shared/utils/error-response";
import { mapKeyValuePairCollectionToControlItems } from "src/app/shared/utils/mapper";

import * as DictionariesActions from './dictionaries.actions';
import { KeyValuePair } from "./dictionaries.models";
import { environment } from "src/environments/environment";

@Injectable()
export class DictionariesEffects {

    constructor(private http: HttpClient, private actions$: Actions, private notification: NotificationService) {}

    loadDictionaries$ = createEffect(() => this.actions$.pipe(
        ofType(DictionariesActions.fetchDictionaries),
        switchMap(() => {
            return zip(
                this.http.get<KeyValuePair[]>(`${environment.trendApiBaseUrl}/SearchWord/GetAvailableSearchEngines`),
                this.http.get<KeyValuePair[]>(`${environment.trendApiBaseUrl}/SearchWord/GetAvailableContextTypes`)
            ).pipe(
                map(([engines, types]) => {
                    const enginesFinal = mapKeyValuePairCollectionToControlItems(engines);
                    const typesFinal = mapKeyValuePairCollectionToControlItems(types);
                    return DictionariesActions.fetchDictionariesSuccess({ engines: enginesFinal, types: typesFinal });
                }),
                catchError((error) => of(DictionariesActions.fetchDictionariesError({ error: handleServerError(error, this.notification) })))
            )
        })
    ));
}