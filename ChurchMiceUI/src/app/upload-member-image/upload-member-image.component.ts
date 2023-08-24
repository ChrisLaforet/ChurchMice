import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UntypedFormBuilder } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AuthService, NotificationService, UploadService } from '@service/index';
import { AuthenticatedUser } from '@data/index';
import { TopBarComponent } from '@app/top-bar/top-bar.component';
import { HttpEventType, HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-upload-member-image',
  templateUrl: './upload-member-image.component.html',
  styleUrls: ['./upload-member-image.component.css']
})
export class UploadMemberImageComponent implements OnInit {
  submitted = false;
  loginName = '';

  constructor(
    private formBuilder: UntypedFormBuilder,
    private uploadService: UploadService
 ) {
  }

  ngOnInit(): void {

  }

  onSubmit(): void {
 }

 // Based upon https://stackoverflow.com/questions/40214772/file-upload-in-angular

  // At the drag drop area
  // (drop)="onDropFile($event)"
  onDropFile(event: DragEvent) {
    event.preventDefault();
    this.uploadFile(event.dataTransfer.files);
  }

  // At the drag drop area
  // (dragover)="onDragOverFile($event)"
  onDragOverFile(event) {
    event.stopPropagation();
    event.preventDefault();
  }

  // At the file input element
  // (change)="selectFile($event)"
  selectFile(event) {
    this.uploadFile(event.target.files);
  }

  uploadFile(files: FileList) {
    if (files.length == 0) {
      console.log("No file selected!");
      return

    }
    let file: File = files[0];

  //
  //     .subscribe(
  //       event => {
  //         if (event.type == HttpEventType.UploadProgress) {
  //           const percentDone = Math.round(100 * event.loaded / event.total);
  //           console.log(`File is ${percentDone}% loaded.`);
  //         } else if (event instanceof HttpResponse) {
  //           console.log('File is completely loaded!');
  //         }
  //       },
  //       (err) => {
  //         console.log("Upload Error:", err);
  //       }, () => {
  //         console.log("Upload done");
  //       }
  //     )
   }
}

