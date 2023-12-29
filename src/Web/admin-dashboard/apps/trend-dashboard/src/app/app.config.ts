import { ApplicationConfig } from '@angular/core';
import { PreloadAllModules, Route, provideRouter, withPreloading } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';

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
];


export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(APP_ROUTES,
      withPreloading(PreloadAllModules)), 
    provideHttpClient(),
  ],
};
