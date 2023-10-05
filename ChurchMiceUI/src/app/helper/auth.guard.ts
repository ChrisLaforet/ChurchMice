import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '@service/index';
import { JwtHelper } from './jwt-helper';

@Injectable({providedIn: 'root'})
export class AuthGuard implements CanActivate {

  constructor(
    private router: Router,
    private authService: AuthService) { }

  // tslint:disable-next-line:typedef
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | boolean | UrlTree {
    const user = this.authService.getAuthenticatedUser();
    if (user === null) {
      this.router.navigate(['login']);
      return false;
    }

    let requiredRoles: string[] = [];
    if (route.data !== null && route.data['roles']) {
      requiredRoles = route.data['roles'];
    }
    return JwtHelper.isUserAuthorizedFor(requiredRoles, user);
  }
}
