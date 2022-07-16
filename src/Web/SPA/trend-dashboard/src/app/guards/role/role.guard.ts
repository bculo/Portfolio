import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Store } from '@ngrx/store';
import { filter, map, Observable, take, tap } from 'rxjs';

import * as fromRoot from 'src/app/store';

import * as authSelector from 'src/app/store/auth/auth.selectors';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {

  constructor(private store: Store<fromRoot.State>, private router: Router){ }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      return this.store.select(authSelector.getAuthState).pipe(
        filter(state => !state.loading),
        take(1),
        tap(state => {
          if(!state.isAuthenticated || state.role != 'Admin'){
            console.log("USER NOT ADMIN");
            this.router.navigate(["/static/home"]);
          }
        }),
        map(state => state.role == 'Admin')
      );
  }
  
}
