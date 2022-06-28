import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SettingsRoutingModule } from './settings-routing.module';
import { SettingsComponent } from './settings.component';
import { ButtonModule } from 'src/app/shared/buttons/button/button.module';
import { SearchWordItemComponent } from './components/search-word-item/search-word-item.component';
import { SearchWordFormComponent } from './components/search-word-form/search-word-form.component';

import { MatDialogModule } from '@angular/material/dialog';
import { ReactiveFormsModule } from '@angular/forms';
import { SelectModule } from 'src/app/shared/controls/select/select.module';
import { FormFiledModule } from 'src/app/shared/controls/form-filed/form-filed.module';
import { InputModule } from 'src/app/shared/controls/input/input.module';

@NgModule({
  declarations: [
    SettingsComponent,
    SearchWordItemComponent,
    SearchWordFormComponent,
  ],
  imports: [
    CommonModule,
    SettingsRoutingModule,
    ButtonModule,
    MatDialogModule,
    ReactiveFormsModule,
    SelectModule,
    FormFiledModule,
    InputModule
  ]
})
export class SettingsModule { }
