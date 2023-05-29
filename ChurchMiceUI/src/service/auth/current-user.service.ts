import { Injectable, OnDestroy } from '@angular/core';
import { AuthService } from '@service/auth/auth.service';
import { AuthenticatedUser, UserDto } from '@data';
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

  public getUserrId(): number {
    if (this.user == null || this.user.Id == null) {
      return -1;
    }
    return parseInt(this.user.Id);
  }

  public getUserRecord(): UserDto | null {
    if (this.user == null || this.user.Id == null) {
      return null;
    }
    return new UserDto(parseInt(this.user.Id), this.user.userFirst, this.user.userLast, true);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
