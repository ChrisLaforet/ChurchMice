import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IApiKeyReaderService } from '@service/index';
import { environment } from '@environments/environment';
import { JwtResponseDto, UserDto } from '@data/index';
import { Observable } from 'rxjs';
import { MessageResponseDto } from '@data/dto/message-response.dto';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly baseUrl: string;
  private readonly allUsersUrl: string;
  private readonly userUrl: string;
  private readonly createUserUrl: string;
  private readonly validateEmailUrl: string;

  private readonly headers: HttpHeaders;

  constructor(private apikeyReaderService: IApiKeyReaderService,
              private http: HttpClient) {
    this.baseUrl = environment.servicesUrl + '/api/user';
    this.allUsersUrl = this.baseUrl;
    this.userUrl = this.baseUrl + '/';
    this.createUserUrl = this.baseUrl + '/createUser';
    this.validateEmailUrl = this.baseUrl + '/validateUserEmail';
    this.headers = new HttpHeaders()
      .set('content-type', 'application/json')
      .set('Access-Control-Allow-Origin', '*')
      .set('apikey', apikeyReaderService.getApiKey());
  }

  public getAllUsers(): Observable<UserDto[]> {
    return this.http.get<UserDto[]>(this.allUsersUrl, {'headers': this.headers});
  }

  public getUserFor(username: string): Observable<UserDto> {
    return this.http.get<UserDto>(this.userUrl + username, {'headers': this.headers});
  }

  public createUserFor(username: string, password: string, email: string, fullname: string): Observable<MessageResponseDto> {
    var userData = {
      "username": username,
      "password": password,
      "fullname" : fullname,
      "email" : email
    };
    return this.http.post<MessageResponseDto>(this.createUserUrl, userData, {'headers': this.headers});
  }

  public validateEmailFor(username: string, password: string): Observable<MessageResponseDto> {
    var userData = {
      "username": username,
      "password": password
    };
    return this.http.post<MessageResponseDto>(this.validateEmailUrl, userData, {'headers': this.headers});
  }
}
