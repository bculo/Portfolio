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
import { ControlItem } from '../shared/models/dictionary.model';
import { DictionaryService } from '../shared/services/open-api/api/dictionary.service';

interface DictionaryState {
    isLoading: boolean
    sortOptions: ControlItem[]
    activeFilterOptions: ControlItem[],
    searchEngines: ControlItem[],
    contextTypes: ControlItem[]
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
        searchEnginesWithoutAll: computed(() => searchEngines().filter(x => x.value !== allKeyValue)),
        contextTypesWithoutAll: computed(() => contextTypes().filter(x => x.value !== allKeyValue)),
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
                                searchEngines: searchEngRes.map(i => ({value: i.key, displayValue: i.value} as ControlItem)),
                                contextTypes: contextTypesRes.map(i => ({value: i.key, displayValue: i.value} as ControlItem)),
                                activeFilterOptions: activeFilterRes.map(i => ({value: i.key, displayValue: i.value} as ControlItem)),
                                sortOptions: sortOptionsRes.map(i => ({value: i.key, displayValue: i.value} as ControlItem)),
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