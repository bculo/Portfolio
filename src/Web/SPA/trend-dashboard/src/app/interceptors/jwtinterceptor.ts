import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Store } from "@ngrx/store";
import { map, Observable, tap, take, switchMap } from "rxjs";

import * as fromRoot from 'src/app/store';

import * as authSelectors from 'src/app/store/auth/auth.selectors';
import { KeycloakService } from "../services/auth/keycloak.service";

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

    constructor(private store: Store<fromRoot.State>, private auth: KeycloakService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {   
        return this.store.select(authSelectors.getAuthState).pipe(
            take(1),
            switchMap(state => {
                if(state.isAuthenticated) {
                    req = req.clone({
                        setHeaders: { Authorization: `Bearer ${this.auth.getAuthorizationToken()}`}
                    })
                }
                return next.handle(req);
            })
        );
    }
}