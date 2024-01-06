import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { NavigationLayoutComponent } from '../../layouts/navigation-layout/navigation-layout.component';
import { SearchWordStore } from './store/search-word-store';

@Component({
  selector: 'admin-dashboard-search-words-route',
  standalone: true,
  imports: [CommonModule, RouterOutlet, NavigationLayoutComponent],
  template: `
    <div>
      <admin-dashboard-navigation-layout>
        <router-outlet></router-outlet>
      </admin-dashboard-navigation-layout>
    </div>`,
})
export class SearchWordsRouteComponent {
  readonly searchWordStore = inject(SearchWordStore);
}
