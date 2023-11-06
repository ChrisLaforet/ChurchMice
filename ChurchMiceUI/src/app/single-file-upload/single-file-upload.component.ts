import { Component, Input, Output, EventEmitter } from '@angular/core';
import { throwError } from 'rxjs';
import { IUploadService } from '@service/utility/upload-service.interface';
import { ReceivedFileContent } from '@service/utility/received-file-content';

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
  @Output() uploadCompleted = new EventEmitter<string>();

  status: 'initial' | 'fetching' | 'fetched' | 'uploading' | 'success' | 'fail' = 'initial';
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
        const fileString = myReader.result as string;
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

      const memberId = this.memberId;
      this.status = 'fetching';
      const fetchFile$ = this.uploadService.receiveFileFromClient(this.file, this.memberId);
      fetchFile$.subscribe({
        next: (fileContent: ReceivedFileContent) => {
          this.status = 'fetched';
          this.uploadReceivedFileContent(fileContent, memberId);
        },
        error: (error: any) => {
          this.status = 'fail';
          return throwError(() => error);
        },
      })
    }
  }

  uploadReceivedFileContent(fileContent: ReceivedFileContent, memberId: number) {
    if (!this.uploadService) {
      return;
    }

    this.status = 'uploading';
    const upload$ = this.uploadService.handleUploadToServer(fileContent, memberId);
    upload$.subscribe({
      next: () => {
        this.status = 'success';
        this.uploadCompleted.emit('upload');
      },
      error: (error: any) => {
        this.status = 'fail';
        return throwError(() => error);
      },
    });
  }
}

