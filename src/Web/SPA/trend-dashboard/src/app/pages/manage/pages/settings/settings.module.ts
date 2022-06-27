import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SettingsRoutingModule } from './settings-routing.module';
import { SettingsComponent } from './settings.component';
import { ButtonModule } from 'src/app/shared/buttons/button/button.module';
import { SearchWordItemComponent } from './components/search-word-item/search-word-item.component';
import { SearchWordFormComponent } from './components/search-word-form/search-word-form.component';


@NgModule({
  declarations: [
    SettingsComponent,
    SearchWordItemComponent,
    SearchWordFormComponent
  ],
  imports: [
    CommonModule,
    SettingsRoutingModule,
    ButtonModule
  ]
})
export class SettingsModule { }
