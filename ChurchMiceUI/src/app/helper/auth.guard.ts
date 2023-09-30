import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import jwt_decode from 'jwt-decode';
import { AuthService } from '@service/index';
import { JwtRoleExtractor } from '@app/helper/jwt-role-extractor';

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
        let requiredRoles: string[] = [];
        if (route.data !== null && route.data['roles']) {
          requiredRoles = route.data['roles'];
        }

        if (requiredRoles.length > 0) {
          let notPermitted = true;
          // @ts-ignore
          const roles = JwtRoleExtractor.getRoles(this.authService.getAuthenticatedUser().token);
          for (const requiredRole of requiredRoles) {
            for (const role of roles) {
              if (requiredRole === role) {
                notPermitted = false;
                break;
              }
            }
            if (!notPermitted) {
              break;
            }
          }

          if (notPermitted) {
            return false;
          }
        }

        return true;
      } else {
        this.authService.logout();
      }
    }
    this.router.navigate(['login']);
    return false;
  }
}
