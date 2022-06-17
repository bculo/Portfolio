import { ActionReducerMap, createFeatureSelector } from '@ngrx/store';

import { DictionariesEffects } from './dictionaries/dictionaries.effects';
import { SyncEffects } from './sync/sync.effects';

import * as fromSync from './sync/sync.reducer';
import * as fromDictionaries from './dictionaries/dictionaries.reducer';

export interface SyncModuleState {
    sync: fromSync.State,
    dictionaries: fromDictionaries.State,
};

export const reducer: ActionReducerMap<SyncModuleState> = {
    sync: fromSync.reducer,
    dictionaries: fromDictionaries.reducer,
};

export const effects: any[] = [
    SyncEffects,
    DictionariesEffects
];

export const syncUserModuleState = createFeatureSelector<SyncModuleState>('sync');


