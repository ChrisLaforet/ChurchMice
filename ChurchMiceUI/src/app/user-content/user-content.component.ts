import { Component } from '@angular/core';
import { ApiKeyReaderService, IApiKeyReaderService } from '@service/index';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-user-content',
  templateUrl: './user-content.component.html',
})
export class UserContentComponent {

  constructor(private apikeyReaderService: IApiKeyReaderService) {

  }

  // TODO: force main screen to login or display what if authenticated?
}
