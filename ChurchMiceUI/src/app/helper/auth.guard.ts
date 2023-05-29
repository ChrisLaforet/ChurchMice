import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import jwt_decode from 'jwt-decode';
import { AuthService } from '@service/index';

@Injectable({providedIn: 'root'})
export class AuthGuard implements CanActivate {

  constructor(
    private router: Router,
    private authService: AuthService
  ) {
  }

  private static validateTokenExpirationIsValid(token: string): boolean {
    try {
      const tokenPayload = jwt_decode(token);
      // @ts-ignore
      const expiration = parseInt(tokenPayload.exp, 10);
      const now = new Date();
      if (now.getTime() / 1000 <= expiration) {
        return true;
      }
      console.log('User token is expired - forcing logout');
      return false;
    } catch (e) {
      console.error('Invalid token format being validated: ' + e);
      return false;
    }
  }

  // tslint:disable-next-line:typedef
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | boolean | UrlTree {
    if (this.authService.getAuthenticatedUser() != null) {
      // @ts-ignore
      if (AuthGuard.validateTokenExpirationIsValid(this.authService.getAuthenticatedUser().token)) {
        return true;
      } else {
        this.authService.logout();
      }
    }
    this.router.navigate(['login']);
    return false;
  }
}
