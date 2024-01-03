import { ApplicationConfig, isDevMode } from '@angular/core';
import { PreloadAllModules, Route, provideRouter, withPreloading } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { BASE_PATH } from './shared/services/open-api';
import { provideStore } from '@ngrx/store';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { jwtInterceptor } from './shared/interceptors/jwt.interceptor';

import { provideIcons, provideNgIconsConfig } from '@ng-icons/core';
import { APP_ROUTES } from './app.routes';
import { APP_ICONS } from './app.icons';


export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(APP_ROUTES,
      withPreloading(PreloadAllModules)), 
    provideHttpClient(
      withInterceptors([jwtInterceptor]),
    ),
    { provide: BASE_PATH, useValue: 'http://localhost:5276' },
    provideIcons(APP_ICONS),
    provideNgIconsConfig({
      size: '1.5em',
    }),
    provideStore(), 
    provideStoreDevtools({
      maxAge: 25,
      logOnly: !isDevMode(),
      connectInZone: true
    }),
  ],
};
