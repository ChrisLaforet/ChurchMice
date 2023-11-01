import { Component, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { throwError } from 'rxjs';
import { IUploadService } from '@service/utility/upload-service.interface';

@Component({

// See https://uploadcare.com/blog/how-to-upload-files-in-angular/ (taken mostly from here)
// and https://blog.angular-university.io/angular-file-upload/
// and for uploading into Angular: https://stackoverflow.com/questions/40843218/upload-a-file-and-read-data-with-filereader-in-angular-2

  selector: 'app-single-file-upload',
  templateUrl: './single-file-upload.component.html',
  styleUrls: ['./single-file-upload.component.css'],
})
export class SingleFileUploadComponent {

  @Input() uploadService: IUploadService | undefined;
  @Input() memberId: number | undefined;

  status: 'initial' | 'uploading' | 'success' | 'fail' = 'initial';
  file: File | null = null;

  constructor() {}

  ngOnInit(): void {}

  public setUploadService(uploadService: IUploadService) {
    this.uploadService = uploadService;
  }

  onChange(event: any) {
    const file: File = event.target.files[0];

    if (file) {
      this.status = 'initial';
      this.file = file;

      const myReader: FileReader = new FileReader();
      var fileType = event.target.parentElement.id;
      myReader.onloadend = function (e) {

console.log(myReader.result);
        const fileString = myReader.result as string;
console.log('Got filestring');
      }
      myReader.readAsText(file);
    }
  }

  onUpload() {
    if (this.file && this.uploadService && this.memberId) {
      // const formData = new FormData();
      // formData.append('file', this.file, this.file.name);
      //
      // const upload$= this.http.post('https://httpbin.org/post', formData);

      const upload$ = this.uploadService.uploadFile(this.file, this.memberId);

      this.status = 'uploading';

      upload$.subscribe({
        next: () => {
          this.status = 'success';
        },
        error: (error: any) => {
          this.status = 'fail';
          return throwError(() => error);
        },
      });
    }
  }
}

