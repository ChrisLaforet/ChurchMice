import { Injectable } from '@angular/core';
import { AuthService } from '@service/auth/auth.service';
import { JwtHelper } from '@app/helper/jwt-helper';
import { Router} from '@angular/router';

@Injectable({providedIn: 'root'})
export class RoleValidator {
  constructor(private router: Router,
              private authService: AuthService) { }

  public validateUserAuthorizedFor(requiredRoles: string[]): void {
    const user = this.authService.getAuthenticatedUser();
    if (user === null ||
        !JwtHelper.isUserAuthorizedFor(requiredRoles, user)) {
      this.router.navigate(['']);
    }
  }
}
