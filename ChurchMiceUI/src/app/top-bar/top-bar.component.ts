import { Component, OnDestroy } from '@angular/core';
import { faBell, faCartShopping, faEllipsisVertical, faEnvelope } from '@fortawesome/free-solid-svg-icons';
import { Subscription } from 'rxjs';
import { AuthenticatedUser } from '@app/model';
import { AuthService } from '@app/service';

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.css']
})
export class TopBarComponent implements OnDestroy {
  faBell = faBell;
  faEnvelope = faEnvelope;
  faCartShopping = faCartShopping;
  faEllipsisVertical = faEllipsisVertical;

  isExpanded = false;

  subscription: Subscription;
  user: AuthenticatedUser | null;

  constructor(private authService: AuthService) {
    this.user = null;
    this.subscription = this.authService.currentAuthenticationState.subscribe(user => this.user = user);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  userName(): string {
    if (this.user == null) {
      return 'Not logged in';
    }
    return this.user.userFirst + ' ' + this.user.userLast;
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
