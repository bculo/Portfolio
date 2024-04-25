import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { NavigationLayoutComponent } from '../../shared/components/navigation-layout/navigation-layout.component';
import { SearchWordStore } from './store/search-word-store';

@Component({
  selector: 'admin-dashboard-search-words-route',
  standalone: true,
  imports: [CommonModule, RouterOutlet, NavigationLayoutComponent],
  template: `<div><router-outlet></router-outlet></div>`,
})
export class SearchWordsRouteComponent {
  readonly searchWordStore = inject(SearchWordStore);
}
