import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IApikeyReaderService } from '@service/apikey-reader.service.interface';
import { environment } from '@environments/environment';
import { MemberDto } from '@app/model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly baseUrl: string;
  private readonly allUsersUrl: string;
  private readonly userUrl: string;

  private readonly headers: HttpHeaders;

  constructor(private apikeyReaderService: IApikeyReaderService,
              private http: HttpClient) {
    this.baseUrl = environment.servicesUrl + '/api/user';
    this.allUsersUrl = this.baseUrl;
    this.userUrl = this.baseUrl + '/';
    this.headers = new HttpHeaders()
      .set('content-type', 'application/json')
      .set('Access-Control-Allow-Origin', '*')
      .set('apikey', apikeyReaderService.getApikey());
//      .set('apikey', apikeyReaderService.getApikey());
  }

  getAllUsers(): Observable<UserDto[]> {
    return this.http.get<UserDto[]>(this.allUsersUrl, {'headers': this.headers});
  }

  getMemberFor(username: string): Observable<MemberDto> {
    return this.http.get<UserDto>(this.userUrl + username, {'headers': this.headers});
  }
}
