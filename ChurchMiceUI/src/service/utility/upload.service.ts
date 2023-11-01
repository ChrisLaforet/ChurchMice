import { HttpEvent, HttpParams, HttpRequest } from '@angular/common/http';
import { Observable, Observer, Subscriber } from "rxjs";
import { IUploadService } from '@service/utility/upload-service.interface';
import { ReceivedFileContent } from '@service/utility/received-file-content';

export abstract class UploadService implements IUploadService {

  constructor() {
  }

  receiveFileFromClient(file: File, id: number): Observable<ReceivedFileContent> {
    let fileReader = new FileReader();
    fileReader.readAsArrayBuffer(file);

    // Based on tarion's response in https://stackoverflow.com/questions/40214772/file-upload-in-angular
    // per karthikaruna's response: https://stackoverflow.com/questions/46513123/angular-return-observable-es6-promise-from-filereader
    return new Observable<ReceivedFileContent>((observer: Subscriber<ReceivedFileContent>) => {
      fileReader.onloadend = (e) => {
        let content = fileReader.result as ArrayBuffer;

        let binary = '';
        let bytes = new Uint8Array(content);
        for (var offset= 0; offset < bytes.byteLength; offset++) {
          binary += String.fromCharCode( bytes[offset] );
        }
        let base64 = window.btoa(binary);

        const receivedFileContent = new ReceivedFileContent(file.name, file.size, file.type, base64);
        observer.next(receivedFileContent);
        observer.complete();
      }
      fileReader.onabort = () => {
        observer.error();
      }
      fileReader.onerror = () => {
        observer.error();
      }
    });
  }

  uploadReceivedFileContentToServer(fileContent: ReceivedFileContent, id: number): Observable<HttpEvent<any>> {
    return this.handleUploadToServer(fileContent, id);
  }

  private readFileContents(file: File): Promise<ArrayBuffer> {
    return new Promise<ArrayBuffer>((resolve, reject) => {
      if (!file) {
        resolve(new ArrayBuffer(0));
      }

      const reader = new FileReader();
      reader.onload = (e) => {
        var result: ArrayBuffer = reader.result as ArrayBuffer;
        resolve(result);
      };

      reader.readAsArrayBuffer(file);
    });
  }

  async getFileContents(file: File) {
    return await this.readFileContents(file);
  }

  abstract handleUploadToServer(fileContent: ReceivedFileContent, id: number): Observable<HttpEvent<any>>;
}

