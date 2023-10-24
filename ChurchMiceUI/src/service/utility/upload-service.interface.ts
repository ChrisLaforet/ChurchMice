import { Observable } from 'rxjs';
import { HttpEvent } from '@angular/common/http';

export interface IUploadService {
  uploadFile(file: File):  Observable<HttpEvent<any>>;
}