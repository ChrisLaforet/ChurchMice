import { AuthService, IApiKeyReaderService } from '@service/index';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { AuthenticatedUser, LocalConfigurationDto } from '@data/index';
import { UserContentListDto } from '@data/configuration/user-content-list.dto';
import { MessageResponseDto } from '@data/dto/message-response.dto';

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService {

  private readonly adminUrl: string;
  private readonly contentUrl: string;

  private readonly contentListUrl: string;
  private readonly getContentUrl: string;
  private readonly configurationUrl: string;
  private readonly setConfigurationUrl: string;

  private readonly headers: HttpHeaders;

  constructor(private apikeyReaderService: IApiKeyReaderService,
              private http: HttpClient) {
    this.adminUrl = environment.servicesUrl + '/api/admin';
    this.contentUrl = environment.servicesUrl + '/api/content';

    this.configurationUrl = this.adminUrl + '/getLocalConfiguration';
    this.setConfigurationUrl = this.adminUrl + '/setLocalConfiguration';
    this.contentListUrl = this.contentUrl;
    this.getContentUrl = this.contentUrl + '/getContent/';

    this.headers = new HttpHeaders()
      .set('content-type', 'application/json')
      .set('Access-Control-Allow-Origin', '*')
      .set('apikey', apikeyReaderService.getApiKey());
  }

  public getConfiguration(): Observable<LocalConfigurationDto> {
    return this.http.get<LocalConfigurationDto>(this.configurationUrl, {'headers': this.headers});
  }

  public getUserContentList(): Observable<UserContentListDto> {
    return this.http.get<UserContentListDto>(this.contentListUrl, {'headers': this.headers});
  }

  public getUserHtmlContent(key: string){
    return this.http.get(this.getContentUrl + key, {'headers': this.headers, responseType : 'text' as const, observe: 'events'});
  }

  public getUserPngContent(key: string){
    return this.http.get(this.getContentUrl + key, {'headers': this.headers, responseType : 'blob', observe: 'events'});
  }

  public saveConfiguration(ministryName: string, baseUrl: string): Observable<MessageResponseDto> {
    const configData = {
      "ministryName": ministryName,
      "baseUrl": baseUrl
    };
    return this.http.put<MessageResponseDto>(this.setConfigurationUrl, configData, {'headers': this.prepareHeaders()});
  }

  private prepareHeaders(): HttpHeaders {
    let newHeaders = new HttpHeaders();
    this.headers.keys().forEach(key => {
      const value = this.headers.get(key);
      if (value !== null) {
        newHeaders = newHeaders.set(key, value);
      }
    });

    let json = localStorage.getItem(AuthService.STORED_AUTHENTICATED_USER);
    if (json != null) {
      try {
        const authenticatedUser = <AuthenticatedUser>JSON.parse(json);
        newHeaders = newHeaders.set('Authorization', 'Bearer ' + authenticatedUser.token);
      } catch (e) {
        console.log('Cannot parse AuthenticatedUser while logging out');
      }
    }
    return newHeaders;
  }
}
