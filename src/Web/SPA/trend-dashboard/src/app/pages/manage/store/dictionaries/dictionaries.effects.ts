import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, of, switchMap, zip } from "rxjs";
import { mapKeyValuePairCollectionToControlItems } from "src/app/shared/utils/mapper";

import * as DictionariesActions from './dictionaries.actions';
import { KeyValuePair } from "./dictionaries.models";

@Injectable()
export class DictionariesEffects {

    constructor(private http: HttpClient, private actions$: Actions) {}

    loadDictionaries$ = createEffect(() => this.actions$.pipe(
        ofType(DictionariesActions.fetchDictionaries),
        switchMap(() => {
            return zip(
                this.http.get<KeyValuePair[]>("http://localhost:5276/api/SearchWord/GetAvaiableSearchEngines"),
                this.http.get<KeyValuePair[]>("http://localhost:5276/api/SearchWord/GetAvaiableContextTypes")
            ).pipe(
                map(([engines, types]) => {
                    const enginesFinal = mapKeyValuePairCollectionToControlItems(engines);
                    const typesFinal = mapKeyValuePairCollectionToControlItems(types);
                    return DictionariesActions.fetchDictionariesSuccess({ engines: enginesFinal, types: typesFinal });
                }),
                catchError((error) => of(DictionariesActions.fetchDictionariesError({ error: error.message })))
            )
        })
    ));
}