import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavigationLayoutComponent } from './shared/components/navigation-layout/navigation-layout.component';
import { KeycloakService } from './shared/services/keycloak/keycloak.service';
import { BackButtonService } from './shared/components/back-button/back-button.service';


@Component({
  standalone: true,
  imports: [RouterModule, NavigationLayoutComponent],
  selector: 'admin-dashboard-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit, OnDestroy {
  readonly authService = inject(KeycloakService);
  readonly backService = inject(BackButtonService);

  ngOnInit(): void {
    this.authService.init();
    this.backService.init();
  } 

  ngOnDestroy(): void {
      this.backService.destroy();
  }
}
