import {HttpEvent} from '@angular/common/http';
import {Observable} from "rxjs";
import {IUploadService} from '@service/utility/upload-service.interface';

export abstract class UploadService implements IUploadService {

  // Based on tarion's response in https://stackoverflow.com/questions/40214772/file-upload-in-angular

  constructor() {
  }

  // file from event.target.files[0]
  uploadFile(file: File, id: number): Observable<HttpEvent<any>> {
    return this.performUpload(file, id);
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

  abstract performUpload(file: File, id: number): Observable<HttpEvent<any>>;
}
