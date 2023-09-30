import { Component, OnDestroy } from '@angular/core';
import {
  faChurch,
  faCircleCheck,
  faCircleQuestion,
  faClock,
  faGear,
  faRightToBracket,
  faRightFromBracket,
  faScaleBalanced,
  faUserPlus,
  faUsers
} from '@fortawesome/free-solid-svg-icons';
import { SafeUrl, DomSanitizer } from '@angular/platform-browser';
import { ConfigurationLoader } from '../../operation';
import { AuthService } from '@service/auth/auth.service';
import { AuthenticatedUser } from '@data/auth/authenticated-user';
import { Subscription } from 'rxjs';
import { JwtRoleExtractor } from '@app/helper';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnDestroy {

  image: SafeUrl | null = null;
  isExpanded = false;

  faRightToBracket = faRightToBracket;
  faRightFromBracket = faRightFromBracket;
  faUserPlus = faUserPlus;
  faChurch = faChurch;
  faCircleQuestion = faCircleQuestion;
  faClock = faClock;
  faScaleBalanced = faScaleBalanced;
  faCircleCheck = faCircleCheck;
  faGear = faGear;
  faUsers = faUsers;

  subscription: Subscription;
  user: AuthenticatedUser | null;

  constructor(private configurationLoader: ConfigurationLoader,
              private authService: AuthService,
              private sanitizer: DomSanitizer) {
    this.user = null;
    this.subscription = this.authService.currentAuthenticationState.subscribe(user => this.user = user);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  private checkHasConfiguredComponent(name: string) : boolean {
    if (this.configurationLoader !== undefined &&
      this.configurationLoader.getConfiguration() !== null) {
      if (name.toUpperCase() === 'ABOUT') {
        return this.configurationLoader.getConfiguration().hasAbout();
      } else if (name.toUpperCase() === 'INDEX') {
        return this.configurationLoader.getConfiguration().hasIndex();
      } else if (name.toUpperCase() === 'BELIEFS') {
        return this.configurationLoader.getConfiguration().hasBeliefs();
      } else if (name.toUpperCase() === 'SERVICES') {
        return this.configurationLoader.getConfiguration().hasServices();
      } else if (name.toUpperCase() === 'LOGO') {
        return this.configurationLoader.getConfiguration().hasLogo();
      }
    }
    return false;
  }

  hasConfiguredImage(): boolean {
    return this.checkHasConfiguredComponent('LOGO');
  }

  getConfiguredImage(): SafeUrl | undefined {
    if (this.configurationLoader !== undefined &&
      this.configurationLoader.getConfiguration() !== null &&
      this.configurationLoader.getConfiguration().hasLogo()) {

      const unsafeImg= URL.createObjectURL(<Blob>this.configurationLoader.getConfiguration().getLogo());
      return this.sanitizer.bypassSecurityTrustUrl(unsafeImg);
    }
    return undefined;
  }

  hasConfiguredPages(): boolean {
    return this.hasAbout() || this.hasIndex() || this.hasBeliefs() || this.hasBeliefs();
  }

  hasAbout(): boolean {
    return this.checkHasConfiguredComponent('ABOUT');
  }

  getAbout(): string {
    if (this.configurationLoader !== undefined &&
      this.configurationLoader.getConfiguration() !== null &&
      this.configurationLoader.getConfiguration().hasAbout()) {
      return this.configurationLoader.getConfiguration().getAbout();
    }
    return '';
  }

  hasIndex(): boolean {
    return this.checkHasConfiguredComponent('INDEX');
  }

  hasBeliefs(): boolean {
    return this.checkHasConfiguredComponent('BELIEFS');
  }

  hasServices(): boolean {
    return this.checkHasConfiguredComponent('SERVICES');
  }

  isLoggedIn(): boolean {
    return this.user !== null;
  }

  isAdministrator(): boolean {
    if (this.user === null || this.user.token === null) {
      return false;
    }
    return JwtRoleExtractor.isUserAnAdministrator(this.user.token);
  }
}
