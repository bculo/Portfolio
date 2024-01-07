import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIconComponent } from '@ng-icons/core';
import { NAVIGATION_ITEMS } from '../../app.routes';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthStore } from '../../store/auth-store';
import { nameOf } from '../../shared/utilities/utilities';
import { ANIMATED_BACKGROUND, APP_ICONS } from '../../app.icons';
import { KeycloakService } from '../../shared/services/keycloak.service';
import { NgxParticlesModule, NgParticlesService } from "@tsparticles/angular";
import { loadSlim } from "@tsparticles/slim"; 

@Component({
  selector: 'admin-dashboard-navigation-layout',
  standalone: true,
  imports: [CommonModule, NgIconComponent, RouterLink, RouterLinkActive, NgxParticlesModule],
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
