import { Routes } from '@angular/router';
import { SearchWordsRouteComponent } from './search-words-route.component';

export const SEARCH_WORDS_ROUTES: Routes = [
    {
      path: '',
      component: SearchWordsRouteComponent,
      children: [
        {
            path: '',
            loadComponent: () => import('./pages/view-page/view-page.component').then(i => i.ViewPageComponent),
        }
      ]
    }
];