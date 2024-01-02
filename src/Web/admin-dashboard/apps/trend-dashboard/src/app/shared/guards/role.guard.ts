import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthStore } from '../../store/auth-store';
import { toObservable } from '@angular/core/rxjs-interop';
import { filter, map, take, tap } from 'rxjs';

export const roleGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authStore = inject(AuthStore);

  console.log("roleGuard called")

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
