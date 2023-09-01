import { Component, Input, OnDestroy } from '@angular/core';
import { faBell, faCartShopping, faEllipsisVertical, faEnvelope } from '@fortawesome/free-solid-svg-icons';
import { Subscription } from 'rxjs';
import { AuthenticatedUser } from '@data/index';
import { AuthService } from '@service/index';
import { ConfigurationLoader } from '../../operation/configuration/configuration-loader';

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.css']
})
export class TopBarComponent implements OnDestroy {

  @Input() configurationLoader?: ConfigurationLoader = undefined;

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

  getUserName(): string {
    if (this.user == null) {
      return 'Not logged in';
    }
    return '' + this.user?.fullName;
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  getMinistryName(): string {
    if (this.configurationLoader !== undefined && this.configurationLoader.GetConfiguration() !== null && this.configurationLoader.GetConfiguration().ministryName !== undefined) {
      return '' + this.configurationLoader.GetConfiguration().ministryName;
    }
    return 'Church Mice';
}
}
