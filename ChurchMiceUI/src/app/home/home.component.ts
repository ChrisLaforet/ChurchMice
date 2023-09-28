import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ConfigurationLoader } from '../../operation';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  constructor(private configurationLoader: ConfigurationLoader,
              private router: Router) {

  }

  ngOnInit(): void {
      if (this.configurationLoader !== undefined &&
        this.configurationLoader.getConfiguration() !== undefined &&
        this.configurationLoader.getConfiguration().hasIndex()) {
        this.router.navigate(['/main']);
      } else {
        this.router.navigate(['/login']);
      }
    }
}
