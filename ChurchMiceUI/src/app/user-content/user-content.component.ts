import { Component, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { ConfigurationLoader } from '../../operation';
import { ReplaySubject, Observable, take } from 'rxjs';

@Component({
  selector: 'app-user-content',
  templateUrl: './user-content.component.html',
})
export class UserContentComponent {

  private pageType: string | undefined = undefined;
  iframeDoc$ = new ReplaySubject<any>(1);

  constructor(private configurationLoader: ConfigurationLoader,
              private route: ActivatedRoute,
              private domSanitizer: DomSanitizer) {
    route.data.pipe()
      .subscribe({
        next: (data) => {
          this.pageType = data.page;
          return;
        }
      });
  }

  checkHasConfiguredComponent() : boolean {
    if (this.configurationLoader !== undefined &&
      this.pageType !== undefined &&
      this.configurationLoader.getConfiguration() !== null) {
      if (this.pageType.toUpperCase() === 'ABOUT') {
        return this.configurationLoader.getConfiguration().hasAbout();
      } else if (this.pageType.toUpperCase() === 'INDEX') {
        return this.configurationLoader.getConfiguration().hasIndex();
      } else if (this.pageType.toUpperCase() === 'BELIEFS') {
        return this.configurationLoader.getConfiguration().hasBeliefs();
      } else if (this.pageType.toUpperCase() === 'SERVICES') {
        return this.configurationLoader.getConfiguration().hasServices();
      } else if (this.pageType.toUpperCase() === 'LOGO') {
        return this.configurationLoader.getConfiguration().hasLogo();
      }
    }
    return false;
  }

  getConfiguredContent(): string {
    if (this.configurationLoader !== undefined &&
      this.pageType !== undefined &&
      this.configurationLoader.getConfiguration() !== null) {
      if (this.pageType.toUpperCase() === 'ABOUT' && this.configurationLoader.getConfiguration().hasAbout()) {
        return this.configurationLoader.getConfiguration().getAbout();
      } else if (this.pageType.toUpperCase() === 'INDEX' && this.configurationLoader.getConfiguration().hasIndex()) {
        return this.configurationLoader.getConfiguration().getIndex();
      } else if (this.pageType.toUpperCase() === 'BELIEFS' && this.configurationLoader.getConfiguration().hasBeliefs()) {
        return this.configurationLoader.getConfiguration().getBeliefs();
      } else if (this.pageType.toUpperCase() === 'SERVICES' && this.configurationLoader.getConfiguration().hasServices()) {
        return this.configurationLoader.getConfiguration().getServices();
      }
    }
    return '';
  }

  loadContent(target: HTMLIFrameElement): void {
    if (target.contentDocument === null) {
      return;
    }
    target.contentDocument.open();
    target.contentDocument.write(this.getConfiguredContent());

    // @ts-ignore
    target.height = String(target.contentWindow.document.body.scrollHeight + 150);
  }
}
