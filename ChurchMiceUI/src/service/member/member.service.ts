import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService, IApiKeyReaderService } from '@service/index';
import { environment } from '@environments/environment';
import { AuthenticatedUser, MemberDto, MemberImageDto, UserDataDto } from '@data/index';
import { Observable } from 'rxjs';
import { MessageResponseDto } from '@data/index';
import { MemberImagesDto } from '@data/dto/member-images.dto';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private readonly baseUrl: string;
  private readonly getMemberUrl: string;
  private readonly createUrl: string;
  private readonly updateUrl: string;
  private readonly deleteUrl: string;
  private readonly getMembersUrl: string;
  private readonly getEditableMembersUrl: string;
  private readonly getMemberImagesUrl: string;

  private readonly headers: HttpHeaders;

  constructor(private apikeyReaderService: IApiKeyReaderService,
              private http: HttpClient) {
    this.baseUrl = environment.servicesUrl + '/api/member';
    this.getMemberUrl = this.baseUrl + '/getMember/';
    this.getMembersUrl = this.baseUrl + '/getMembers';
    this.getEditableMembersUrl = this.baseUrl + '/getEditableMembers';
    this.createUrl = this.baseUrl + '/create';
    this.updateUrl = this.baseUrl + '/update';
    this.deleteUrl = this.baseUrl + '/delete/';
    this.getMemberImagesUrl = this.baseUrl + '/getImages/';
    this.headers = new HttpHeaders()
      .set('content-type', 'application/json')
      .set('Access-Control-Allow-Origin', '*')
      .set('apikey', apikeyReaderService.getApiKey());
  }

  public getMember(memberId: string): Observable<MemberDto | null> {
    return this.http.get<MemberDto>(this.getMemberUrl + memberId, {'headers': this.prepareHeaders()});
  }

  public createMember(firstName: string, lastName: string, email?: string,
                      homePhone?: string, mobilePhone?: string, mailingAddress1?: string,
                      mailingAddress2?: string, city?: string, state?: string, zip?: string,
                      birthday?: string, anniversary?: string, memberSince?: string,
                      userId?: string): Observable<MemberDto> {
    const userData = {
      "firstName": firstName,
      "lastName": lastName,
      "email": email,
      "homePhone": homePhone,
      "mobilePhone": mobilePhone,
      "mailingAddress1": mailingAddress1,
      "mailingAddress2": mailingAddress2,
      "city": city,
      "state": state,
      "zip": zip,
      "birthday": birthday,
      "anniversary": anniversary,
      "userId": userId,
      "memberSince": memberSince
    };
    return this.http.post<MemberDto>(this.createUrl, userData, {'headers': this.prepareHeaders()});
  }

  public updateMember(member: MemberDto): Observable<MemberDto> {
    const userData = {
      "id": member.id,
      "firstName": member.firstName,
      "lastName": member.lastName,
      "email": member.email,
      "homePhone": member.homePhone,
      "mobilePhone": member.mobilePhone,
      "mailingAddress1": member.mailingAddress1,
      "mailingAddress2": member.mailingAddress2,
      "city": member.city,
      "state": member.state,
      "zip": member.zip,
      "birthday": member.birthday,
      "anniversary": member.anniversary,
      "userId": member.userId,
      "memberSince": member.memberSince
    };
    return this.http.put<MemberDto>(this.updateUrl, userData, {'headers': this.prepareHeaders()});
  }

  public getAllMembers(): Observable<MemberDto[]> {
    return this.http.get<MemberDto[]>(this.getMembersUrl, {'headers': this.prepareHeaders()});
  }

  public getAllEditableMembers(): Observable<MemberDto[]> {
    return this.http.get<MemberDto[]>(this.getEditableMembersUrl, {'headers': this.prepareHeaders()});
  }

  public deleteMember(memberId: string): Observable<MessageResponseDto> {
    return this.http.delete<MessageResponseDto>(this.deleteUrl + memberId, {'headers': this.prepareHeaders()});
  }

  public getMemberImages(memberId: string): Observable<MemberImagesDto> {
    return this.http.get<MemberImagesDto>(this.getMemberImagesUrl + memberId, {'headers': this.prepareHeaders()});
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
