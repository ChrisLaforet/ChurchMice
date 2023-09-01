import { IApiKeyReaderService } from '@service/index';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { LocalConfigurationDto } from '@data/index';
import { UserContentListDto } from '@data/configuration/user-content-list.dto';

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService {

  private readonly adminUrl: string;
  private readonly contentUrl: string;

  private readonly contentListUrl: string;
  private readonly getContentUrl: string;
  private readonly configurationUrl: string;

  private readonly headers: HttpHeaders;

  constructor(private apikeyReaderService: IApiKeyReaderService,
              private http: HttpClient) {
    this.adminUrl = environment.servicesUrl + '/api/admin';
    this.contentUrl = environment.servicesUrl + '/api/content';

    this.configurationUrl = this.adminUrl;
    this.contentListUrl = this.contentUrl;
    this.getContentUrl = this.contentUrl + '/getContent/';

    this.headers = new HttpHeaders()
      .set('content-type', 'application/json')
      .set('Access-Control-Allow-Origin', '*')
      .set('apikey', apikeyReaderService.getApiKey());
  }

  getConfiguration(): Observable<LocalConfigurationDto> {
    return this.http.get<LocalConfigurationDto>(this.configurationUrl, {'headers': this.headers});
  }

  getUserContentList(): Observable<UserContentListDto> {
    return this.http.get<UserContentListDto>(this.contentListUrl, {'headers': this.headers});
  }

  getUserHtmlContent(key: string){
    return this.http.get(this.getContentUrl + key, {'headers': this.headers, responseType : 'text' as const, observe: 'events'});
  }

  getUserPngContent(key: string){
    return this.http.get(this.getContentUrl + key, {'headers': this.headers, responseType : 'blob', observe: 'events'});
  }
}
