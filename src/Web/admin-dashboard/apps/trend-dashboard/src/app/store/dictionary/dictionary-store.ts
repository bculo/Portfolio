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
import { KeyValuePair } from './dictionary-store.model';
import { DictionaryService } from '../../shared/services/open-api/api/dictionary.service';
import { ControlItem } from '../../shared/controls/controls.model';
import { ActiveEnumOptions, ContextTypeEnumOptions, SearchEngineEnumOptions } from '../../shared/enums/enums';

interface DictionaryState {
    isLoading: boolean,
    sortOptions: KeyValuePair[]
    activeOptions: KeyValuePair[],
    searchEngines: KeyValuePair[],
    contextTypes: KeyValuePair[]
}

const initialState: DictionaryState = {
    isLoading: true,
    sortOptions: [],
    activeOptions: [],
    searchEngines: [],
    contextTypes: [],
}

export const DictionaryStore = signalStore(
    { providedIn: 'root' },
    withState(initialState),
    withDevtools('dictionary'),
    withComputed(({ searchEngines, contextTypes, sortOptions, activeOptions }) => ({
        contextTypesFilterItemsOptions: computed(() => contextTypes().map(x => ({...x} as ControlItem))),
        searchEngineFilterItemsOptions: computed(() => searchEngines().map(x => ({...x} as ControlItem))),
        activeFilterItemsOptions: computed(() => activeOptions().map(x => ({...x} as ControlItem))),
        sortFilterItemsOptions: computed(() => sortOptions().map(x => ({...x} as ControlItem))),
        
        contextTypeEditItemsOptions: computed(() => 
            contextTypes().filter(x => x.value !== ContextTypeEnumOptions.All).map(x => ({...x} as ControlItem))),
        searchEngineEditItemsOptions: computed(() => 
            searchEngines().filter(x => x.value !== SearchEngineEnumOptions.All).map(x => ({...x} as ControlItem))),
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