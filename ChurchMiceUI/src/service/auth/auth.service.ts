import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '@environments/environment';
import { AsyncSubject, BehaviorSubject, Observable } from 'rxjs';
import { AuthenticatedUser, JwtContent, JwtResponseDto } from '@data/index';
import { first } from 'rxjs/operators';
import jwt_decode from 'jwt-decode';
import { IApikeyReaderService, NotificationService } from '@service/index';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly baseUrl: string;
  private readonly loginUrl: string;
  private readonly logoutUrl: string;

  private readonly headers: HttpHeaders;

  public currentAuthenticationState: BehaviorSubject<AuthenticatedUser | null> = new BehaviorSubject<AuthenticatedUser | null>(null);

  public static STORED_AUTHENTICATED_USER = 'AMGRUser';

  private authenticationState?: AsyncSubject<AuthenticatedUser>;
  private authenticatedUser?: AuthenticatedUser;

  constructor(private apikeyReaderService: IApikeyReaderService,
              private http: HttpClient,
              private notifyService: NotificationService) {
    this.baseUrl = environment.servicesUrl + '/api/user';
    this.loginUrl = this.baseUrl + '/login';
    this.logoutUrl = this.baseUrl + '/logout';
    this.headers = new HttpHeaders()
      .set('content-type', 'application/json')
      .set('Access-Control-Allow-Origin', '*')
      .set('apikey', apikeyReaderService.getApikey());
  }

  public login(email: string, password: string): Observable<AuthenticatedUser> {

    if (this.authenticationState != null) {
      this.authenticationState.error('Login cancelled');
    }
    localStorage.removeItem(AuthService.STORED_AUTHENTICATED_USER);
    this.authenticatedUser = undefined;
    this.currentAuthenticationState.next(null);

    this.authenticationState = new AsyncSubject<AuthenticatedUser>();

    this.doLogin(email, password)
      .pipe(first())
      .subscribe({
        next: (jwtResponse) => {
          var decoded = jwt_decode<JwtContent>(jwtResponse.token);
          console.log('Login success ' + decoded.user + ' (' + jwtResponse.firstName + ' ' + jwtResponse.lastName + ') with token id ' + decoded.ser);
          this.authenticatedUser = new AuthenticatedUser(jwtResponse.token, decoded.sub, decoded.user, decoded.user, jwtResponse.firstName, jwtResponse.lastName, decoded.member);
          localStorage.setItem(AuthService.STORED_AUTHENTICATED_USER, JSON.stringify(this.authenticatedUser));

          this.authenticationState?.next(this.authenticatedUser);
          this.authenticationState?.complete();

          this.currentAuthenticationState.next(this.authenticatedUser);
        },
        error: (err: any) => {
          this.notifyService.showError('Error encountered while logging in', 'Login failed');
          console.log('Error while logging in');
          console.log(err);

          this.authenticationState?.error('Login failed');
        }
      });

    return this.authenticationState;
  }

  private doLogin(email: string, password: string): Observable<JwtResponseDto> {
    var credentials = {
      "email": email,
      "password": password
    };
    return this.http.post<JwtResponseDto>(this.loginUrl, credentials, {'headers': this.headers});
  }

  public logout() {
    localStorage.removeItem(AuthService.STORED_AUTHENTICATED_USER);
    this.authenticatedUser = undefined;
    this.currentAuthenticationState.next(null);
  }

  public getAuthenticatedUser(): AuthenticatedUser | null {
    // TODO: handle user from local storage
    if (this.authenticatedUser == null) {
      let json = localStorage.getItem(AuthService.STORED_AUTHENTICATED_USER);
      if (json == null) {
        this.currentAuthenticationState.next(null);
        return null;
      }
      try {
        this.authenticatedUser = <AuthenticatedUser>JSON.parse(json);
      } catch (e) {
        console.error('Unable to parse AuthenticatedUser from saved data - removing!');
        localStorage.removeItem(AuthService.STORED_AUTHENTICATED_USER);

        this.currentAuthenticationState.next(null);
        return null;
      }
    }

    this.currentAuthenticationState.next(this.authenticatedUser);

    return this.authenticatedUser;
  }
}
