import { Injectable, OnDestroy } from '@angular/core';
import { AuthService } from '@app/service';
import { AuthenticatedUser, MemberDto } from '@app/model';
import { Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CurrentMemberService implements OnDestroy {

  subscription: Subscription;
  user: AuthenticatedUser | null;

  constructor(private authService: AuthService) {
    this.user = null;
    this.subscription = this.authService.currentAuthenticationState.subscribe(user => this.user = user);
  }

  public getMemberId(): number {
    if (this.user == null || this.user.memberId == null) {
      return -1;
    }
    return parseInt(this.user.memberId);
  }

  public getMemberRecord(): MemberDto | null {
    if (this.user == null || this.user.memberId == null) {
      return null;
    }
    return new MemberDto(parseInt(this.user.memberId), this.user.userFirst, this.user.userLast, true);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
