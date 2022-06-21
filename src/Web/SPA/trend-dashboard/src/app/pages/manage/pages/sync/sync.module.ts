import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SyncRoutingModule } from './sync-routing.module';
import { ButtonModule } from 'src/app/shared/buttons/button/button.module';
import { ListComponent } from './pages/list/list.component';
import { ListItemComponent } from './pages/list/components/list-item/list-item.component';
import { DetailComponent } from './pages/detail/detail.component';


@NgModule({
  declarations: [
    ListComponent,
    ListItemComponent,
    DetailComponent
  ],
  imports: [
    CommonModule,
    SyncRoutingModule,
    ButtonModule
  ]
})
export class SyncModule { }
