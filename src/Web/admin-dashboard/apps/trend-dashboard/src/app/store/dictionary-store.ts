import {
    signalStore,
    patchState,
    withMethods,
    withState,
    withHooks,
    withComputed,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { computed, inject } from '@angular/core';
import { mergeMap, pipe, switchMap, zip } from 'rxjs';
import { tapResponse } from '@ngrx/operators';
import { withDevtools } from '@angular-architects/ngrx-toolkit'
import { KeyValuePair } from '../shared/models/dictionary.model';
import { DictionaryService } from '../shared/services/open-api/api/dictionary.service';

interface DictionaryState {
    isLoading: boolean
    sortOptions: KeyValuePair[]
    activeFilterOptions: KeyValuePair[],
    searchEngines: KeyValuePair[],
    contextTypes: KeyValuePair[]
}

const initialState: DictionaryState = {
    isLoading: true,
    sortOptions: [],
    activeFilterOptions: [],
    searchEngines: [],
    contextTypes: [],
}

const allKeyValue = 999;

export const DictionaryStore = signalStore(
    { providedIn: 'root' },
    withState(initialState),
    withDevtools('dictionary'),
    withComputed(({ searchEngines, contextTypes }) => ({
        searchEnginesWithoutAll: computed(() => searchEngines().filter(x => x.id !== allKeyValue)),
        contextTypesWithoutAll: computed(() => contextTypes().filter(x => x.id !== allKeyValue)),
    })),
    withMethods((store, service = inject(DictionaryService)) => ({
        load: rxMethod<void>(
            switchMap(() => 
                zip(
                    service.getActiveFilterOptions(), 
                    service.getContextTypes(),
                    service.getSearchEngines(),
                    service.getSortFilterOptions(),
                ).pipe(
                    tapResponse({
                        next: ([activeFilterRes, contextTypesRes, searchEngRes, sortOptionsRes]) => 
                            patchState(store, { 
                                searchEngines: searchEngRes.map(i => ({id: i.key, value: i.value} as KeyValuePair)),
                                contextTypes: contextTypesRes.map(i => ({id: i.key, value: i.value} as KeyValuePair)),
                                activeFilterOptions: activeFilterRes.map(i => ({id: i.key, value: i.value} as KeyValuePair)),
                                sortOptions: sortOptionsRes.map(i => ({id: i.key, value: i.value} as KeyValuePair)),
                            }),
                        error: console.error,
                        finalize: () => patchState(store, {isLoading: false})
                    })    
                )
            ),        
        ),
    })),
    withHooks({
        onInit({ load }) {
            load()
        },
    }),
);