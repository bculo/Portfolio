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
      <admin-dashboard-empty-layout *ngIf="!authenticated()">
        <router-outlet></router-outlet>
      </admin-dashboard-empty-layout>
      <admin-dashboard-navigation-layout *ngIf="authenticated()">
        <router-outlet></router-outlet>
      </admin-dashboard-navigation-layout>
    </div>`,
})
export class StaticRouteComponent {
  readonly store = inject(AuthStore);
  authenticated = this.store.isAuthenticated;
}

