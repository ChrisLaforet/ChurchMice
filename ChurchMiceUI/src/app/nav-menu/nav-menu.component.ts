import { Component } from '@angular/core';
import {
  faBucket,
  faCakeCandles,
  faChartLine,
  faClipboardList,
  faCoins,
  faCommentDollar,
  faCubesStacked,
  faFilePen,
  faGauge,
  faGear,
  faHouseUser,
  faRightToBracket,
  faTachometerAlt,
  faTemperatureHalf,
  faTractor,
  faVideo
} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  faHouseUser = faHouseUser;
  faTachometerAlt = faTachometerAlt;
  faGear = faGear;
  faVideo = faVideo;
  faBucket = faBucket;
  faCakeCandles = faCakeCandles;
  faClipboardList = faClipboardList;
  faFilePen = faFilePen;
  faCubesStacked = faCubesStacked;
  faRightToBracket = faRightToBracket;
  faCoins = faCoins;
  faGauge = faGauge;
  faTractor = faTractor;
  faChartLine = faChartLine;
  faCommentDollar = faCommentDollar;
  faTemperatureHalf = faTemperatureHalf;


  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
