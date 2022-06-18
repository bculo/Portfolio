import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ManageRoutingModule } from './manage-routing.module';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

import { effects, reducer } from './store';
import { SyncItemComponent } from './components/sync-item/sync-item.component'
import { ButtonModule } from 'src/app/shared/buttons/button/button.module';
import { ReactiveFormsModule } from '@angular/forms';
import { InputModule } from 'src/app/shared/controls/input/input.module';
import { FormFiledModule } from 'src/app/shared/controls/form-filed/form-filed.module';
import { SelectModule } from 'src/app/shared/controls/select/select.module';

import { SYNC_MODULE_STATE } from './store';
import { ManageComponent } from './manage.component';

@NgModule({
  declarations: [
    ManageComponent,
    SyncItemComponent
  ],
  imports: [
    CommonModule,
    ButtonModule,
    ReactiveFormsModule,
    InputModule,
    FormFiledModule,
    SelectModule,

    StoreModule.forFeature(SYNC_MODULE_STATE, reducer),
    EffectsModule.forFeature(effects),

    ManageRoutingModule,
  ]
})
export class ManageModule { }
