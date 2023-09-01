import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IApiKeyReaderService } from '@service/index';
import { environment } from '@environments/environment';
import { UserDto } from '@data/index';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly baseUrl: string;
  private readonly allUsersUrl: string;
  private readonly userUrl: string;

  private readonly headers: HttpHeaders;

  constructor(private apikeyReaderService: IApiKeyReaderService,
              private http: HttpClient) {
    this.baseUrl = environment.servicesUrl + '/api/user';
    this.allUsersUrl = this.baseUrl;
    this.userUrl = this.baseUrl + '/';
    this.headers = new HttpHeaders()
      .set('content-type', 'application/json')
      .set('Access-Control-Allow-Origin', '*')
      .set('apikey', apikeyReaderService.getApiKey());
  }

  getAllUsers(): Observable<UserDto[]> {
    return this.http.get<UserDto[]>(this.allUsersUrl, {'headers': this.headers});
  }

  getUserFor(username: string): Observable<UserDto> {
    return this.http.get<UserDto>(this.userUrl + username, {'headers': this.headers});
  }
}
