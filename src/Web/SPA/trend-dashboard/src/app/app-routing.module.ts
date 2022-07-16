import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoleGuard } from './guards/role/role.guard';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'static/welcome'
  },
  {
    path: 'news',
    loadChildren: () => import('./pages/news/news.module').then(i => i.NewsModule),
    canActivate: [RoleGuard]
  },
  {
    path: 'manage',
    loadChildren: () => import('./pages/manage/manage.module').then(i => i.ManageModule),
    canActivate: [RoleGuard]
  },
  {
    path: 'static',
    loadChildren: () => import('./pages/static/static.module').then(i => i.StaticModule)
  },
  {
    path: '**',
    pathMatch: 'full',
    redirectTo: 'static/home',
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
