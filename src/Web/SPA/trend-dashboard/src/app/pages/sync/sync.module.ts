import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SyncRoutingModule } from './sync-routing.module';
import { SyncComponent } from './sync.component';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

import { reducers, effects } from './store';
import { SyncItemComponent } from './components/sync-item/sync-item.component'

@NgModule({
  declarations: [
    SyncComponent,
    SyncItemComponent
  ],
  imports: [
    CommonModule,
    SyncRoutingModule,

    StoreModule.forFeature('syncfeature', reducers),
    EffectsModule.forFeature(effects)
  ]
})
export class SyncModule { }
