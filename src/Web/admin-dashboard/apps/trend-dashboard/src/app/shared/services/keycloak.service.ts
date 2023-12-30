import { Injectable } from '@angular/core';

import Keycloak, { KeycloakConfig } from 'keycloak-js';
import { catchError, from, of, take, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class KeycloakService {

  private keycloackInstance: Keycloak | null = null;

  constructor() { }


  private getConfig(): KeycloakConfig {
    return {
      url: "http://localhost:8080/",
      realm: "PortfolioRealm",
      clientId: "Trend.Client"
    }
  }

  init() {
    this.keycloackInstance = new Keycloak(this.getConfig());

    from(this.keycloackInstance.init({ 
      onLoad: 'check-sso', 
      silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html'
    })).pipe(
      take(1), 
      tap((auth) => console.log(auth)),
    ).subscribe()
  }

  log() {
    console.log(this.keycloackInstance?.tokenParsed)
  }

  public login() {
    if(this.isAuthenticated()) {
      return;
    }
    this.keycloackInstance!.login();
  }

  getUserName(): string | null {
    if(this.isAuthenticated())
      return this.keycloackInstance!.tokenParsed!["preferred_username"];
    return null;
  }

  isAuthenticated(): boolean {
    if (this.keycloackInstance)
      return this.keycloackInstance.authenticated ?? false;
    return false;
  }

  getIdToken(): string | null {
    if(this.isAuthenticated())
      return this.keycloackInstance!.idToken ?? null;
    return null;
  }

  getRefreshToken(): string | null {
    if(this.isAuthenticated())
      return this.keycloackInstance!.refreshToken ?? null;
    return null;
  }

  getAuthorizationToken(): string | null{
    if(this.isAuthenticated())
      return this.keycloackInstance!.token ?? null;
    return null;
  }
}
