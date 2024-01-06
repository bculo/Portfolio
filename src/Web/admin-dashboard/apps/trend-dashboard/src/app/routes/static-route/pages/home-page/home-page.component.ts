import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { KeycloakService } from 'apps/trend-dashboard/src/app/shared/services/keycloak.service';
import { AuthStore } from 'apps/trend-dashboard/src/app/store/auth-store';
import { Router } from '@angular/router';

@Component({
  selector: 'admin-dashboard-home-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss',
})
export class HomePageComponent {
  readonly service = inject(KeycloakService);
  readonly authStore = inject(AuthStore);
  readonly router = inject(Router);
  
  isAuth = this.authStore.isAuthenticated
}
