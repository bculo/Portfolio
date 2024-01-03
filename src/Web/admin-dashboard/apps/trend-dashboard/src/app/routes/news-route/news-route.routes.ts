import { Routes } from '@angular/router';
import { NewsRouteComponent } from './news-route.component';

export const NEWS_ROUTES: Routes = [
    {
      path: '',
      component: NewsRouteComponent,
      children: [
        {
          path: '',
          loadComponent: () => import('./pages/view-page/view-page.component').then(i => i.ViewPageComponent)
        },
      ]
    }
  ];