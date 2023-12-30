import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { KeycloakService } from './shared/services/keycloak.service';

@Component({
  standalone: true,
  imports: [RouterModule],
  selector: 'admin-dashboard-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {

  constructor(private service: KeycloakService) {

  }

  ngOnInit(): void {
    this.service.init()
  }
}
