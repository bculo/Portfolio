import { Routes } from '@angular/router';
import { StaticRouteComponent } from './static-route.component';

export const STATIC_ROUTES: Routes = [
    {
      path: '',
      component: StaticRouteComponent,
      children: [
        {
          path: '',
          loadComponent: () => import('./pages/home-page/home-page.component').then(i => i.HomePageComponent)
        },
        {
          path: '404',
          loadComponent: () => import('./pages/not-found-page/not-found-page.component').then(i => i.NotFoundPageComponent),
        }
      ]
    }
  ];