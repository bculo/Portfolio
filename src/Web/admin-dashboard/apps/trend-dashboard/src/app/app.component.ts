import { Component, OnInit, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { KeycloakService } from './shared/services/keycloak.service';
import { NewsService } from './shared/services/open-api';
import { take } from 'rxjs';
import { AuthStore } from './store/auth-store';

@Component({
  standalone: true,
  imports: [RouterModule],
  selector: 'admin-dashboard-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  private keycloak = inject(KeycloakService);
  public authStore = inject(AuthStore);

  ngOnInit(): void {
    console.log("TEST")
    this.keycloak.init()
  }
}
