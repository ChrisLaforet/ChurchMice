import { Observable } from 'rxjs';
import { HttpEvent } from '@angular/common/http';
import { ReceivedFileContent } from '@service/utility/received-file-content';

export interface IUploadService {
  receiveFileFromClient(file: File, id: number): Observable<ReceivedFileContent>;
  handleUploadToServer(fileContent: ReceivedFileContent, id: number): Observable<HttpEvent<any>>;
}
