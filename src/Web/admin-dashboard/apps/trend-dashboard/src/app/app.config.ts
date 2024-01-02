import { ApplicationConfig, isDevMode } from '@angular/core';
import { PreloadAllModules, Route, provideRouter, withPreloading } from '@angular/router';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptors } from '@angular/common/http';
import { BASE_PATH } from './shared/services/open-api';
import { provideStore } from '@ngrx/store';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { roleGuard } from './shared/guards/role.guard';
import { jwtInterceptor } from './shared/interceptors/jwt.interceptor';

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
    path: 'news',
    loadChildren: () => import('./pages/news-route/news-route.routes').then(i => i.NEWS_ROUTES),
    canActivate: [roleGuard]
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
    provideHttpClient(
      withInterceptors([jwtInterceptor]),
    ),
    { provide: BASE_PATH, useValue: 'http://localhost:5276' },
    provideStore(), 
    provideStoreDevtools({
      maxAge: 25,
      logOnly: !isDevMode(),
      connectInZone: true
    }),
  ],
};
