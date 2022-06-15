import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SyncRoutingModule } from './sync-routing.module';
import { SyncComponent } from './sync.component';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

import { reducers, effects } from './store';
import { SyncItemComponent } from './components/sync-item/sync-item.component'
import { ButtonModule } from 'src/app/shared/buttons/button/button.module';
import { ReactiveFormsModule } from '@angular/forms';
import { InputModule } from 'src/app/shared/controls/input/input.module';
import { FormFiledModule } from 'src/app/shared/controls/form-filed/form-filed.module';

@NgModule({
  declarations: [
    SyncComponent,
    SyncItemComponent
  ],
  imports: [
    CommonModule,
    SyncRoutingModule,
    ButtonModule,
    ReactiveFormsModule,
    InputModule,
    FormFiledModule,

    StoreModule.forFeature('syncfeature', reducers),
    EffectsModule.forFeature(effects)
  ]
})
export class SyncModule { }
