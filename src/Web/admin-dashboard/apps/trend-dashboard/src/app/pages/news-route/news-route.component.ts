import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationLayoutComponent } from '../../layouts/navigation-layout/navigation-layout.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'admin-dashboard-news-route',
  standalone: true,
  imports: [CommonModule, NavigationLayoutComponent, RouterOutlet],
  template: `
      <admin-dashboard-navigation-layout>
        <router-outlet></router-outlet>
      </admin-dashboard-navigation-layout>
    `,
})
export class NewsRouteComponent {}
