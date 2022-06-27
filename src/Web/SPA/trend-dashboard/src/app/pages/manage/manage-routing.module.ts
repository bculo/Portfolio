import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'sync',
    loadChildren: () => import('./pages/sync/sync.module').then(i => i.SyncModule)
  },
  {
    path: 'settings',
    loadChildren: () => import('./pages/settings/settings.module').then(i => i.SettingsModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageRoutingModule { }
