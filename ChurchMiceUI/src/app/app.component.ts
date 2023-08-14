import { Component, OnDestroy, OnInit } from '@angular/core';
import { IApiKeyReaderService } from '@service/index';
import { AuthService } from '@service/index';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {

  title = 'ChurchMice';

  constructor(private apikeyReaderService: IApiKeyReaderService,
              private authService: AuthService) {
    apikeyReaderService.getApiKey();
  }

  ngOnInit(): void {
    this.authService.getAuthenticatedUser();    // pump local storage on return
  }

  ngOnDestroy(): void {

  }
}
