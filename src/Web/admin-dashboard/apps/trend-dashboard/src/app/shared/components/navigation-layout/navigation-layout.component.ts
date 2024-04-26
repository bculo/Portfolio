import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIconComponent } from '@ng-icons/core';
import { NAVIGATION_ITEMS } from '../../../app.routes';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthStore } from '../../../store/auth/auth-store';
import { nameOf } from '../../utilities/utilities';
import { ANIMATED_BACKGROUND, APP_ICONS } from '../../../app.icons';
import { KeycloakService } from '../../services/keycloak/keycloak.service';
import { NgxParticlesModule, NgParticlesService } from "@tsparticles/angular";
import { loadSlim } from "@tsparticles/slim"; 
import { WebSocketComponentComponent } from '../web-socket/web-socket-component.component';

@Component({
  selector: 'admin-dashboard-navigation-layout',
  standalone: true,
  imports: [CommonModule, NgIconComponent, RouterLink, RouterLinkActive, NgxParticlesModule, WebSocketComponentComponent],
  templateUrl: './navigation-layout.component.html',
  styleUrl: './navigation-layout.component.scss',
})
export class NavigationLayoutComponent implements OnInit {
  readonly items = [...NAVIGATION_ITEMS];
  readonly authStore = inject(AuthStore);
  readonly keycloak = inject(KeycloakService);
  readonly particalService = inject(NgParticlesService);
  readonly particleSettings = ANIMATED_BACKGROUND;

  isAuthenticated = this.authStore.isAuthenticated;
  userName = this.authStore.userName;
  userNameFirstChar = this.authStore.userNameFirstChar
  userNameProfile = nameOf(() => APP_ICONS.heroPower)

  logout() {
    this.keycloak.logout();
  }

  ngOnInit(): void {
    this.particalService.init(async (engine) => {
      await loadSlim(engine)
    });
  }
}
