import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { KeycloakService } from '../services/keycloak.service';
import { AuthStore } from '../../store/auth-store';
import { toObservable } from '@angular/core/rxjs-interop';
import { filter, map, take, tap } from 'rxjs';

export const roleGuard: CanActivateFn = (route, state) => {
  const keycloak = inject(KeycloakService);
  const router = inject(Router);

  return toObservable(inject(AuthStore).isLoading).pipe(
    filter(isLoading => !isLoading),
    take(1),
    tap(() => {
      const isAuthenticated = keycloak.isAuthenticated();
      if(!isAuthenticated) {
        router.navigate(["/static"]);
      }
    }),
    map(() => keycloak.isInRole("Admin"))
  ) 
};
