import { ActionReducerMap, createFeatureSelector } from '@ngrx/store';
import * as fromSync from './sync';
import { SyncEffects } from './sync/sync.effects';

export interface SyncAppState {
    sync: fromSync.SyncState
};

export const reducers: ActionReducerMap<SyncAppState> = {
    sync: fromSync.syncReducer
};

export const effects: any[] = [
    SyncEffects
];

export const getSyncModuleState = createFeatureSelector<SyncAppState>('syncfeature');