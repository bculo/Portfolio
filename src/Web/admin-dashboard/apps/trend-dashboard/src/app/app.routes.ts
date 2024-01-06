import { Route } from '@angular/router';
import { roleGuard } from './shared/guards/role.guard';
import { nameOf } from './shared/utilities/utilities';
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
      path: 'words',
      loadChildren: () => import('./routes/search-words-route/search-words-route.routes').then(i => i.SEARCH_WORDS_ROUTES),
      canActivate: [roleGuard]
    },
    {
      path: '**',
      redirectTo: 'static/404'
    }
];

export interface NavigationItem {
    path: string,
    name: string,
    icon: string,
    onlyAdminRole: boolean
}

export const NAVIGATION_ITEMS: NavigationItem[] = [
    {
        path: '/static',
        icon: nameOf(() => APP_ICONS.heroHome),
        name: 'Home',
        onlyAdminRole: false
    },
    {
        path: '/news',
        icon: nameOf(() => APP_ICONS.heroNewspaper),
        name: 'News',
        onlyAdminRole: true
    },
    {
      path: '/words',
      icon: nameOf(() => APP_ICONS.heroChatBubbleLeftEllipsis),
      name: 'Words',
      onlyAdminRole: true
  },
]