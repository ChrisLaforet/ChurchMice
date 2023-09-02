import { Component, Input, OnInit } from '@angular/core';
import {
  faRightToBracket,
  faUserPlus
} from '@fortawesome/free-solid-svg-icons';
import { ConfigurationLoader } from '../../operation/configuration/configuration-loader';
import { SafeUrl, DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  @Input() configurationLoader?: ConfigurationLoader = undefined;

  image: SafeUrl | null = null;
  isExpanded = false;

  faRightToBracket = faRightToBracket;
  faUserPlus = faUserPlus;

  constructor(private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
  if (this.configurationLoader !== null &&
    this.configurationLoader?.GetConfiguration() !== null &&
    this.configurationLoader?.GetConfiguration().hasLogo())
    {
      const unsafeImg= URL.createObjectURL(<Blob>this.configurationLoader?.GetConfiguration().getLogo());
      this.image = this.sanitizer.bypassSecurityTrustUrl(unsafeImg);
    }
    console.log(this.image);
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
