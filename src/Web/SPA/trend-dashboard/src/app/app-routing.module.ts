import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'static/welcome'
  },
  {
    path: 'news',
    loadChildren: () => import('./pages/news/news.module').then(i => i.NewsModule)
  },
  {
    path: 'sync',
    loadChildren: () => import('./pages/sync/sync.module').then(i => i.SyncModule)
  },
  {
    path: 'static',
    loadChildren: () => import('./pages/static/static.module').then(i => i.StaticModule)
  },
  {
    path: '**',
    pathMatch: 'full',
    redirectTo: 'static/welcome',
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
