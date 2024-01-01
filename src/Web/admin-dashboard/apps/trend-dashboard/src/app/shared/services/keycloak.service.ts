import { Injectable, inject } from '@angular/core';

import Keycloak, { KeycloakConfig } from 'keycloak-js';
import { catchError, finalize, from, of, take, tap } from 'rxjs';
import { AuthStore } from '../../store/auth-store';

@Injectable({
  providedIn: 'root'
})
export class KeycloakService {

  private keycloackInstance: Keycloak | null = null;
  private authStore = inject(AuthStore);

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
      silentCheckSsoFallback: false,
      silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html',
    })).pipe(
      take(1), 
      tap((auth) => this.authStore.setAuth(auth)),
      finalize(() => this.authStore.setLoading(false))
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

  isInRole(role: string): boolean {
    if(!this.isAuthenticated())
      return false;
    return this.keycloackInstance!.hasRealmRole(role)
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
