import { ApplicationConfig } from '@angular/core';
import { PreloadAllModules, Route, provideRouter, withPreloading } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { BASE_PATH } from './shared/services/open-api';

export const APP_ROUTES: Route[] = [
  {
      path: '',
      pathMatch: 'full',
      redirectTo: 'static'
  },
  {
      path: 'static',
      loadChildren: () => import('./pages/static-route/static-route.routes').then(i => i.STATIC_ROUTES),
  },
  {
    path: '**',
    redirectTo: 'static/404'
  }
];


export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(APP_ROUTES,
      withPreloading(PreloadAllModules)), 
    provideHttpClient(),
    { provide: BASE_PATH, useValue: 'http://localhost:5276' },
  ],
};
