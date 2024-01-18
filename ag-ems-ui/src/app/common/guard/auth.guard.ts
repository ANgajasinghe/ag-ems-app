import {CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";
import {TokenService} from "../services/token.service";

export const authGuard: CanActivateFn = (route, state) => {
  const isAuth = inject(TokenService).isTokenExpired()
  if(!isAuth) {
    inject(Router).navigate(['/login']);
  }

  return isAuth;
};
