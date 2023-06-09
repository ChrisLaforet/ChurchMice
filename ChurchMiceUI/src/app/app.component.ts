import { Component, OnDestroy, OnInit } from '@angular/core';
import { IApikeyReaderService } from '@service/index';
import { AuthService } from '@service/index';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {

  title = 'ChurchMice';

  constructor(private apikeyReaderService: IApikeyReaderService,
              private authService: AuthService) {
    apikeyReaderService.getApikey();
  }

  ngOnInit(): void {
    this.authService.getAuthenticatedUser();    // pump local storage on return
  }

  ngOnDestroy(): void {

  }
}
