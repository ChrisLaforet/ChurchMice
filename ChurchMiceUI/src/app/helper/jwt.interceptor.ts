import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '@service/index';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(public authService: AuthService) {
  }

  // tslint:disable-next-line:no-any
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // add auth header with jwt if user is logged in and request is to the api url
    const isApiUrl = !request.url.includes("/api/user/");
    if (this.authService.getAuthenticatedUser() != null && isApiUrl) {
      // @ts-ignore
      let token = this.authService.getAuthenticatedUser().token;
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(request);
  }
}
