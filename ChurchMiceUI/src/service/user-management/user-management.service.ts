import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService, IApiKeyReaderService } from '@service/index';
import { environment } from '@environments/environment';
import { AuthenticatedUser, UserDataDto } from '@data/index';
import { Observable } from 'rxjs';
import { MessageResponseDto } from '@data/dto/message-response.dto';

@Injectable({
  providedIn: 'root'
})
export class UserManagementService {
  private readonly baseUrl: string;
  private readonly allUsersUrl: string;
  private readonly singleUserUrl: string;
  private readonly changePasswordUrl: string;
  private readonly createUserUrl: string;
  private readonly updateUserUrl: string;
  private readonly setUserRoleUrl: string;

  private readonly headers: HttpHeaders;

  constructor(private apikeyReaderService: IApiKeyReaderService,
              private http: HttpClient) {
    this.baseUrl = environment.servicesUrl + '/api/admin';
    this.allUsersUrl = this.baseUrl + '/getUsers';
    this.singleUserUrl = this.baseUrl + '/getUser/';
    this.createUserUrl = this.baseUrl + '/createUser';
    this.updateUserUrl = this.baseUrl + '/updateUser';
    this.changePasswordUrl = this.baseUrl + '/setUserPassword';
    this.setUserRoleUrl = this.baseUrl + '/setUserRole';

    this.headers = new HttpHeaders()
      .set('content-type', 'application/json')
      .set('Access-Control-Allow-Origin', '*')
      .set('apikey', apikeyReaderService.getApiKey());
  }

  public getAllUsers(): Observable<UserDataDto[]> {
    return this.http.get<UserDataDto[]>(this.allUsersUrl, {'headers': this.prepareHeaders()});
  }

  public getUser(userId: string): Observable<UserDataDto | null> {
    return this.http.get<UserDataDto>(this.singleUserUrl + userId, {'headers': this.prepareHeaders()});
  }

  public setUserRole(userId: string, roleLevelCode: string): Observable<any> {
    const data = {
      "userId": userId,
      "roleLevelCode": roleLevelCode
    };
    return this.http.put<any>(this.setUserRoleUrl, data, {'headers': this.prepareHeaders()});
  }

  public createUser(username: string, fullname: string, email: string): Observable<MessageResponseDto> {
    const userData = {
      "username": username,
      "fullname" : fullname,
      "email" : email
    };
    return this.http.post<MessageResponseDto>(this.createUserUrl, userData, {'headers': this.prepareHeaders()});
  }

  public updateUser(userId: string, fullname: string, email: string): Observable<MessageResponseDto> {
    const userData = {
      "userId": userId,
      "fullname" : fullname,
      "email" : email
    };
    return this.http.put<MessageResponseDto>(this.updateUserUrl, userData, {'headers': this.prepareHeaders()});
  }

  public setUserPassword(userId: string, password: string): Observable<MessageResponseDto> {
    const userData = {
      "userId": userId,
      "password" : password
    };
    return this.http.put<MessageResponseDto>(this.changePasswordUrl, userData, {'headers': this.prepareHeaders()});
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
