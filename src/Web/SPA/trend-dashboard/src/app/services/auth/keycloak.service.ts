import { Injectable } from '@angular/core';
import Keycloak from 'keycloak-js';

import json from 'src/assets/keycloak.json';

import { catchError, from, of, take, tap } from 'rxjs';
import { Store } from '@ngrx/store';

import * as fromRoot from 'src/app/store/';

import * as authActions from 'src/app/store/auth/auth.actions';
import { UserAuthenticated } from 'src/app/store/auth/auth.models';
import { NotificationService } from '../notification/notification.service';

@Injectable({
  providedIn: 'root'
})
export class KeycloakService {

  private keycloackInstance: Keycloak;
  private initCalled: boolean;

  constructor(private store: Store<fromRoot.State>, 
    private notification: NotificationService) {}

  private getConfig(): any {
    return {
      url: json['auth-server-url'],
      realm: json['realm'],
      clientId: json['resource']
    }
  }

  private userInValidRole(): boolean {
    return this.keycloackInstance.hasRealmRole("Admin");
  }

  private handleAuthentication(authenticated: boolean): void {
    console.log(this.keycloackInstance)
    if(!authenticated){
      this.store.dispatch(authActions.userAuthenticationFailed());
      return;
    }

    const storeInstance: UserAuthenticated = {
      email: this.getEmail(),
      username: this.getUserName(),
      role: !this.userInValidRole() ? 'User' : 'Admin'
    };

    this.store.dispatch(authActions.userAuthenticated( {status:storeInstance} ));

    if(storeInstance.role == 'User')
      this.notification.error("User has no rights to use Trend.Client application");
  }

  private addEventListeners(){
    this.keycloackInstance.onTokenExpired = () => {
      this.refreshToken();
    }
  }

  private refreshToken() {
    from(this.keycloackInstance.updateToken(5)).pipe(
      take(1),
      tap((refreshed) => this.handleRefreshResponse(refreshed)),
      catchError((error) => { this.clearToken(); return of(null) })
    ).subscribe();
  }

  private handleRefreshResponse(refreshed: boolean) {
    if(!refreshed) {
      this.store.dispatch(authActions.userLogout());
      return;
    }
  }

  init() {
    if(this.initCalled) return;

    this.store.dispatch(authActions.userAuthenticationStarted());

    this.keycloackInstance = new Keycloak(this.getConfig());

    from(this.keycloackInstance.init({ 
      onLoad: 'check-sso', 
      silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html'
    })).pipe(take(1), tap((auth) => this.handleAuthentication(auth))).subscribe();

    this.addEventListeners();

    this.initCalled = true;
  }

  getEmail(): string {
    if(this.keycloackInstance.authenticated && this.keycloackInstance.idTokenParsed["email"])
      return this.keycloackInstance.idTokenParsed["email"];
    return null;
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

  register(): void {
    this.keycloackInstance.register();
  }

  logout(): void {
    this.keycloackInstance.logout({ redirectUri: window.location.origin + "/static/home" });
  }

  getKeycloackInstance(): Keycloak {
    return this.keycloackInstance;
  }
}

