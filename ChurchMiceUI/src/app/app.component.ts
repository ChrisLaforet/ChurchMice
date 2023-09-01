import { Component, OnDestroy, OnInit } from '@angular/core';
import { IApiKeyReaderService } from '@service/index';
import { AuthService } from '@service/index';
import { ConfigurationLoader } from '../operation/configuration/configuration-loader';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {

  constructor(private apikeyReaderService: IApiKeyReaderService,
              private authService: AuthService,
              private configurationLoader: ConfigurationLoader) {
    apikeyReaderService.getApiKey();
  }

  ngOnInit(): void {
    this.authService.getAuthenticatedUser();    // pump local storage on return
  }

  ngOnDestroy(): void {

  }

  getConfigurationLoader(): ConfigurationLoader {
    return this.configurationLoader;
  }
}
