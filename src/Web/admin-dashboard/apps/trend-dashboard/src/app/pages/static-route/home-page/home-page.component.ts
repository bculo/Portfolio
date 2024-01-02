import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthStore } from '../../../store/auth-store';
import { KeycloakService } from '../../../shared/services/keycloak.service';

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
  
  isAuth = this.authStore.isAuthenticated
  userName = this.authStore.userName

  constructor() {}
}
