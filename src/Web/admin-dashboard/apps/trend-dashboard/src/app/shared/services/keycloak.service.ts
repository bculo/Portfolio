import { Injectable, inject } from '@angular/core';

import Keycloak, { KeycloakConfig } from 'keycloak-js';
import { Observable, catchError, finalize, from, map, of, take, tap } from 'rxjs';


export interface ConfigureResponse {
  isAuthenticated: boolean,
  userInfo: AuthenticatedUserInfo | null
}

export interface AuthenticatedUserInfo {
  userName: string,
  isAdmin: boolean,
  token: string,
  refreshToken: string,
  idToken: string,
  email: string
}


@Injectable({
  providedIn: 'root'
})
export class KeycloakService {

  private keycloackInstance: Keycloak | null = null;

  configure() : Observable<ConfigureResponse> {
    this.keycloackInstance = new Keycloak(this.getConfig());
    return from(this.keycloackInstance.init({ 
      onLoad: 'check-sso', 
      silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html',
    })).pipe(
      tap((authStatus) => console.log("Keycloak instance initialized")),
      map((authStatus) => {
        return {
          isAuthenticated: authStatus,
          userInfo: {
            idToken: this.getIdToken(),
            token: this.getAuthorizationToken(),
            refreshToken: this.getRefreshToken(),
            isAdmin: this.isInRole('Admin'),
            userName: this.getUserName(),
            email: this.getEmail()
          }
        } as ConfigureResponse
      })
    )
  }

  public login() {
    if(this.isAuthenticated())
      return;
    this.keycloackInstance!.login();
  }

  public logout() {
    if(!this.isAuthenticated())
      return;
    this.keycloackInstance!.logout();
  }

  private getConfig(): KeycloakConfig {
    return {
      url: "http://localhost:8080/",
      realm: "PortfolioRealm",
      clientId: "Trend.Client"
    }
  }

  private getUserName(): string | null {
    if(this.isAuthenticated())
      return this.keycloackInstance!.tokenParsed!["preferred_username"];
    return null;
  }

  private getEmail(): string | null {
    if(this.isAuthenticated())
      return this.keycloackInstance!.tokenParsed!["email"];
    return null;
  }

  private isInRole(role: string): boolean {
    if(!this.isAuthenticated())
      return false;
    return this.keycloackInstance!.hasRealmRole(role)
  }

  private isAuthenticated(): boolean {
    if (this.keycloackInstance)
      return this.keycloackInstance.authenticated ?? false;
    return false;
  }

  private getIdToken(): string | null {
    if(this.isAuthenticated())
      return this.keycloackInstance!.idToken ?? null;
    return null;
  }

  private getRefreshToken(): string | null {
    if(this.isAuthenticated())
      return this.keycloackInstance!.refreshToken ?? null;
    return null;
  }

  private getAuthorizationToken(): string | null{
    if(this.isAuthenticated())
      return this.keycloackInstance!.token ?? null;
    return null;
  }
}
