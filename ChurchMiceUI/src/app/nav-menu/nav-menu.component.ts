import { Component, Input } from '@angular/core';
import {
  faRightToBracket,
  faUserPlus
} from '@fortawesome/free-solid-svg-icons';
import { ConfigurationLoader } from '../../operation/configuration/configuration-loader';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  @Input() configurationLoader?: ConfigurationLoader = undefined;

  isExpanded = false;

  faRightToBracket = faRightToBracket;
  faUserPlus = faUserPlus;


  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
