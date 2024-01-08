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
import { switchMap, zip } from 'rxjs';
import { tapResponse } from '@ngrx/operators';
import { withDevtools } from '@angular-architects/ngrx-toolkit'
import { KeyValuePair } from '../shared/models/dictionary.model';
import { DictionaryService } from '../shared/services/open-api/api/dictionary.service';
import { ControlItem } from '../shared/models/controls.model';

interface DictionaryState {
    isLoading: boolean,
    defaultAllValue: number,
    sortOptions: KeyValuePair[]
    activeOptions: KeyValuePair[],
    searchEngines: KeyValuePair[],
    contextTypes: KeyValuePair[]
}

const initialState: DictionaryState = {
    isLoading: true,
    defaultAllValue: 999,
    sortOptions: [],
    activeOptions: [],
    searchEngines: [],
    contextTypes: [],
}

export const DictionaryStore = signalStore(
    { providedIn: 'root' },
    withState(initialState),
    withDevtools('dictionary'),
    withComputed(({ searchEngines, contextTypes, sortOptions, activeOptions, defaultAllValue }) => ({
        contextTypesFilterItemsOptions: computed(() => contextTypes().map(x => ({...x, isDefault: x.value === defaultAllValue()} as ControlItem))),
        searchEngineFilterItemsOptions: computed(() => searchEngines().map(x => ({...x, isDefault: x.value === defaultAllValue()} as ControlItem))),
        activeFilterItemsOptions: computed(() => activeOptions().map(x => ({...x, isDefault: x.value === defaultAllValue()} as ControlItem))),
        sortFilterItemsOptions: computed(() => sortOptions().map(x => ({...x} as ControlItem))),
    })),
    withMethods((store, service = inject(DictionaryService)) => ({
        load: rxMethod<void>(
            switchMap(() => 
                zip(
                    service.getDefaultAllValue(),
                    service.getActiveFilterOptions(), 
                    service.getContextTypes(),
                    service.getSearchEngines(),
                    service.getSortFilterOptions(),
                ).pipe(
                    tapResponse({
                        next: ([defaultAllValueRes, activeFilterRes, contextTypesRes, searchEngRes, sortOptionsRes]) => 
                            patchState(store, { 
                                defaultAllValue: defaultAllValueRes,
                                searchEngines: searchEngRes.map(i => ({value: i.key, displayValue: i.value} as KeyValuePair)),
                                contextTypes: contextTypesRes.map(i => ({value: i.key, displayValue: i.value} as KeyValuePair)),
                                activeOptions: activeFilterRes.map(i => ({value: i.key, displayValue: i.value} as KeyValuePair)),
                                sortOptions: sortOptionsRes.map(i => ({value: i.key, displayValue: i.value} as KeyValuePair)),
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