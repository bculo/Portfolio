import { Injectable } from '@angular/core';
import Keycloak from 'keycloak-js';

import json from 'src/assets/keycloak.json';

import { from, take, tap } from 'rxjs';
import { Store } from '@ngrx/store';

import * as fromRoot from 'src/app/store/';

import * as authActions from 'src/app/store/auth/auth.actions';
import { UserAuthenticated } from 'src/app/store/auth/auth.models';

@Injectable({
  providedIn: 'root'
})
export class KeycloakService {

  private keycloackInstance: Keycloak;

  constructor(private store: Store<fromRoot.State>) {}

  private getConfig(): any {
    return {
      url: json['auth-server-url'],
      realm: json['realm'],
      clientId: json['resource']
    }
  }

  private handleAuthentication(authenticated: boolean): void {
    if(!authenticated) return;
    const storeInstance: UserAuthenticated = {
      email: null,
      username: this.getUserName()
    };
    this.store.dispatch(authActions.userAuthenticated({status:storeInstance}));
  }

  init() {
    this.keycloackInstance = new Keycloak(this.getConfig());
    return from(this.keycloackInstance.init({ 
      onLoad: 'check-sso', 
      silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html'
    })).pipe(take(1), tap((auth) => this.handleAuthentication(auth))).subscribe();
  }

  getUserName(): string {
    if(this.keycloackInstance.authenticated)
      return this.keycloackInstance.tokenParsed["preferred_username"];
    return null;
  }

  isAuthenticated(): boolean {
    return this.keycloackInstance.authenticated;
  }

  getIdToken(): string {
    if(this.keycloackInstance.authenticated)
      return this.keycloackInstance.idToken;
    return null;
  }

  getRefreshToken(): string {
    if(this.keycloackInstance.authenticated)
      return this.keycloackInstance.refreshToken;
    return null;
  }

  getAuthorizationToken(): string {
    if(this.keycloackInstance.authenticated)
    return this.keycloackInstance.token;
  return null;
  }

  clearToken(): void {
    this.keycloackInstance.clearToken();
  }

  login(): void {
    this.keycloackInstance.login();
  }

  getKeycloackInstance(): Keycloak {
    return this.keycloackInstance;
  }
}

