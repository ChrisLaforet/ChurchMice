import { Injectable, OnDestroy } from '@angular/core';
import { AuthService } from '@service/auth/auth.service';
import { AuthenticatedUser, UserDto } from '@data/index';
import { Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserService implements OnDestroy {

  subscription: Subscription;
  user: AuthenticatedUser | null;

  constructor(private authService: AuthService) {
    this.user = null;
    this.subscription = this.authService.currentAuthenticationState.subscribe(user => this.user = user);
  }

  public getUserId(): number {
    if (this.user == null || this.user.id == null) {
      return -1;
    }
    return parseInt(this.user.id);
  }

  public getUserRecord(): UserDto | null {
    if (this.user == null || this.user.id == null) {
      return null;
    }
    return new UserDto(parseInt(this.user.id), this.user.fullName, true);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
