import { Injectable } from '@angular/core';
import {HttpClient, HttpParams, HttpRequest, HttpEvent} from '@angular/common/http';
import {Observable} from "rxjs";
import { IUploadService } from '@service/utility/upload-service.interface';

export abstract class UploadService implements IUploadService {

  // Based on tarion's response in https://stackoverflow.com/questions/40214772/file-upload-in-angular

  constructor() {
  }

  // file from event.target.files[0]
  uploadFile(file: File): Observable<HttpEvent<any>> {

    return this.performUpload(file);
  }

  abstract performUpload(file: File): Observable<HttpEvent<any>>;
}
