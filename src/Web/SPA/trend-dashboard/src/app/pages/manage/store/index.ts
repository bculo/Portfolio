import { ActionReducerMap } from '@ngrx/store';

import { DictionariesEffects } from './dictionaries/dictionaries.effects';
import { SyncEffects } from './sync/sync.effects';
import { SettingsEffects } from './settings/settings.effects';

import * as fromSync from './sync/sync.reducer';
import * as fromDictionaries from './dictionaries/dictionaries.reducer';
import * as fromSettings from './settings/settings.reducer';

export interface SyncModuleState {
    sync: fromSync.State,
    dictionaries: fromDictionaries.State,
    settings: fromSettings.State
};

export const reducer: ActionReducerMap<SyncModuleState> = {
    sync: fromSync.reducer,
    dictionaries: fromDictionaries.reducer,
    settings: fromSettings.reducer
};

export const effects: any[] = [
    SyncEffects,
    DictionariesEffects,
    SettingsEffects
];


