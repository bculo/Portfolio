import { Injectable, inject } from '@angular/core';

import Keycloak, { KeycloakConfig } from 'keycloak-js';
import { catchError, finalize, from, map, of, take, tap, throwError } from 'rxjs';
import { AuthStore } from '../../../store/auth/auth-store';
import { AuthWrapper } from '../../../store/auth/auth-store.model';

//http://localhost:8080/realms/PortfolioRealm/protocol/openid-connect/auth?client_id=Stock.Client&redirect_uri=http%3A%2F%2Flocalhost%3A4200%2F&response_type=code&scope=openid&state=8b96f47a645449dbb07759dd6af24c67&code_challenge=E5OPtzdHEeSG9jAns5adieMVfOxuLAt7_u_EX7agi3E&code_challenge_method=S256&response_mode=query

@Injectable({
  providedIn: 'root'
})
export class KeycloakService {
  readonly authStore = inject(AuthStore);
  private keycloackInstance: Keycloak | null = null;

  init(): void {
    this.keycloackInstance = new Keycloak(this.getConfig());

    from(this.keycloackInstance.init({ 
      onLoad: 'check-sso', 
      pkceMethod: 'S256',
      enableLogging: true,
      silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html',
    })).pipe(
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
        } as AuthWrapper
      }),
      tap((authInfo) => this.authStore.set(authInfo)),
      finalize(() => {
        this.authStore.setLoadingFlag(false);
        this.addEventListeners();
      })
    ).subscribe();
  
  } 

  public login() {
    if(this.isAuthenticated())
      return;
    console.log("HELLO")
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

  private addEventListeners(){
    if(!this.isAuthenticated()) {
      return;
    }

    this.keycloackInstance!.onTokenExpired = () => {
      this.refreshToken();
    }
  }

  private refreshToken() {
    from(this.keycloackInstance!.updateToken(5)).pipe(
      take(1),
      tap((authStatus) => console.log("Refresh token")),
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
        } as AuthWrapper
      }),
      tap((info) => this.authStore.set(info)),
      catchError((error) => {
        this.keycloackInstance?.clearToken(); 
        return throwError(() => error);
      })
    ).subscribe();
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
