import { Component, Input } from '@angular/core';
import {
  faChurch,
  faCircleQuestion,
  faClock,
  faRightToBracket,
  faScaleBalanced,
  faUserPlus
} from '@fortawesome/free-solid-svg-icons';
import { SafeUrl, DomSanitizer } from '@angular/platform-browser';
import { ConfigurationLoader } from '../../operation';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  @Input() configurationLoader?: ConfigurationLoader = undefined;

  image: SafeUrl | null = null;
  isExpanded = false;

  faRightToBracket = faRightToBracket;
  faUserPlus = faUserPlus;
  faChurch = faChurch;
  faCircleQuestion = faCircleQuestion;
  faClock = faClock;
  faScaleBalanced = faScaleBalanced;

  constructor(private sanitizer: DomSanitizer) { }

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
}
