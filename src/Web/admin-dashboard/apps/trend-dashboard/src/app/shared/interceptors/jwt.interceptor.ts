import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthStore } from '../../store/auth-store';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authStore = inject(AuthStore);

  if(!authStore.isAuthenticated()) {
    return next(req);
  }

  req = req.clone({
    setHeaders: { Authorization: `Bearer ${authStore.authToken()}`}
  })
  return next(req);
};
