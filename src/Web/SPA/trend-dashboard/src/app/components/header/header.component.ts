import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { KeycloakService } from 'src/app/services/auth/keycloak.service';

import * as fromRoot from 'src/app/store/';
import * as authSelectors from 'src/app/store/auth/auth.selectors';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  isAuthenticated$: Observable<boolean>;

  constructor(private store: Store<fromRoot.State>, private auth: KeycloakService) { }

  ngOnInit(): void {
    this.isAuthenticated$ = this.store.select(authSelectors.isAuthenticated);
  }

  login(): void {
    this.auth.login();
  }

  logout(): void {
    this.auth.logout();
  }
}
