import { Component, OnInit, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavigationLayoutComponent } from './shared/components/navigation-layout/navigation-layout.component';
import { KeycloakService } from './shared/services/keycloak/keycloak.service';


@Component({
  standalone: true,
  imports: [RouterModule, NavigationLayoutComponent],
  selector: 'admin-dashboard-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  readonly authService = inject(KeycloakService);

  ngOnInit(): void {
    this.authService.init();
  } 
}
