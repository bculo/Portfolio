import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { EmptyLayoutComponent } from '../../layouts/empty-layout/empty-layout.component';
import { AuthStore } from '../../store/auth-store';
import { NavigationLayoutComponent } from '../../layouts/navigation-layout/navigation-layout.component';

@Component({
  selector: 'admin-dashboard-static-route',
  standalone: true,
  imports: [CommonModule, RouterOutlet, EmptyLayoutComponent, NavigationLayoutComponent],
  template: `
    <div>
      <admin-dashboard-navigation-layout>
        <router-outlet></router-outlet>
      </admin-dashboard-navigation-layout>
    </div>`,
})
export class StaticRouteComponent {
  readonly store = inject(AuthStore);
  authenticated = this.store.isAuthenticated;
}

