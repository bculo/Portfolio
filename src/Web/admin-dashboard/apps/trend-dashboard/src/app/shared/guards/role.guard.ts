import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthStore } from '../../store/auth/auth-store';
import { toObservable } from '@angular/core/rxjs-interop';
import { filter, map, take, tap } from 'rxjs';

export const roleGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authStore = inject(AuthStore);

  return toObservable(authStore.isLoading).pipe(
    filter(isLoading => !isLoading),
    take(1),
    tap(() => {
      if(!authStore.isAuthenticated()) {
        router.navigate(["/static"]);
      }
    }),
    map(() => authStore.isAdmin())
  ) 
};
