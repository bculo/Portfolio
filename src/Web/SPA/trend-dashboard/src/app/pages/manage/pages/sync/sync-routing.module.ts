import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SyncResolver } from '../../resolvers/sync/sync.resolver';
import { DetailComponent } from './pages/detail/detail.component';
import { ListComponent } from './pages/list/list.component';

const routes: Routes = [
  {
    path: '',
    component: ListComponent
  },
  {
    path: ':id',
    component: DetailComponent,
    resolve: [SyncResolver]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SyncRoutingModule { }
