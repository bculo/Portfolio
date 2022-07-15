import { Component, OnInit } from '@angular/core';
import { KeycloakService } from './services/auth/keycloak.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor(private service: KeycloakService) { }

  ngOnInit(): void {
    this.service.init();
  }
}
