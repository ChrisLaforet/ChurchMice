import { Component } from '@angular/core';
import { ApiKeyReaderService, IApiKeyReaderService } from '@service/index';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  constructor(private apikeyReaderService: IApiKeyReaderService) {

  }

  // TODO: force main screen to login or display what if authenticated?
}
