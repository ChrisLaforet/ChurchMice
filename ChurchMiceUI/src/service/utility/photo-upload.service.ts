import { Injectable } from '@angular/core';
import { IUploadService } from '@service/utility/upload-service.interface';
import { UploadService } from '@service/utility/upload.service';
import { Observable } from 'rxjs';
import { HttpClient, HttpEvent, HttpHeaders, HttpParams, HttpRequest } from '@angular/common/http';
import { environment } from '@environments/environment';
import { IApiKeyReaderService } from '@service/key-support/api-key-reader.service.interface';
import { AuthService } from '@service/auth/auth.service';
import { AuthenticatedUser } from '@data/auth/authenticated-user';

@Injectable({
  providedIn: 'root'
})
export class PhotoUploadService extends UploadService implements IUploadService {

  private readonly uploadUrl: string;
  private readonly headers: HttpHeaders;

  constructor(private apikeyReaderService: IApiKeyReaderService,
              private http: HttpClient) {
    super();
    this.uploadUrl = environment.servicesUrl + '/api/member/uploadImage';
    this.headers = new HttpHeaders()
      .set('content-type', 'application/json')
      .set('Access-Control-Allow-Origin', '*')
      .set('apikey', apikeyReaderService.getApiKey());
  }

  performUpload(file: File, id: number): Observable<HttpEvent<any>> {
    // let formData = new FormData();
    // file.name
    // formData.append('fileContent', file);
    // formData.append('memberId', id.toString());

    let fileReader = new FileReader();
    fileReader.onloadend = (e) => {
      let content = fileReader.result as ArrayBuffer;

      let binary = '';
      let bytes = new Uint8Array(content);
      for (var i = 0; i < bytes.byteLength; i++) {
        binary += String.fromCharCode( bytes[ i ] );
      }
      let base64 = window.btoa(binary);

      const userData = {
        "memberId": id,
        "fileName": file.name,
        "fileSize": file.size,
        "fileType": file.type,
        "fileContent": base64
      };

      let params = new HttpParams();

      const options = {
        params: params,
        reportProgress: true,
        headers: this.prepareHeaders(),
      };

      const req = new HttpRequest('POST', this.uploadUrl, userData, options);
      return this.http.request(req);
    }
    fileReader.readAsArrayBuffer(file);
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
