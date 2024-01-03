import { Route } from '@angular/router';
import { roleGuard } from './shared/guards/role.guard';
import { APP_ICONS } from './app.icons';

export const APP_ROUTES: Route[] = [
    {
      path: '',
      pathMatch: 'full',
      redirectTo: 'static'
    },
    {
      path: 'static',
      loadChildren: () => import('./routes/static-route/static-route.routes').then(i => i.STATIC_ROUTES),
    },
    {
      path: 'news',
      loadChildren: () => import('./routes/news-route/news-route.routes').then(i => i.NEWS_ROUTES),
      canActivate: [roleGuard]
    },
    {
      path: '**',
      redirectTo: 'static/404'
    }
];

const nameOf = (f: () => string) => {
  const result = (f).toString().replace(/[ |\(\)=>]/g,'')
  if(result.indexOf('.')) {
    return result.split(".").pop() ?? ''
  }
  return result
};

export interface NavigationItem {
    path: string,
    name: string,
    icon: string
}

export const NAVIGATION_ITEMS: NavigationItem[] = [
    {
        path: '/static',
        icon: nameOf(() => APP_ICONS.heroHome),
        name: 'Home'
    },
    {
        path: '/news',
        icon: nameOf(() => APP_ICONS.heroNewspaper),
        name: 'News'
    },
]