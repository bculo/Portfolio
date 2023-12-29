import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { EmptyLayoutComponent } from '../../layouts/empty-layout/empty-layout.component';

@Component({
  selector: 'admin-dashboard-static-route',
  standalone: true,
  imports: [CommonModule, RouterOutlet, EmptyLayoutComponent],
  template: `
    <admin-dashboard-empty-layout>
      <router-outlet></router-outlet>
    </admin-dashboard-empty-layout>`,
})
export class StaticRouteComponent {}

