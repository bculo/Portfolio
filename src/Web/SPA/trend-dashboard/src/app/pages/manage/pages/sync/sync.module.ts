import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SyncRoutingModule } from './sync-routing.module';
import { SyncComponent } from './sync.component';


@NgModule({
  declarations: [
    SyncComponent
  ],
  imports: [
    CommonModule,
    SyncRoutingModule
  ]
})
export class SyncModule { }
