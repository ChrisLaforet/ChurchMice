import { Injectable } from '@angular/core';
import { IRecaptchaKeyReaderService } from '@service/index';
import RecaptchaKeyJson from '@assets/recaptcha-key.json';

@Injectable({
  providedIn: 'root'
})
export class RecaptchaKeyReaderService implements IRecaptchaKeyReaderService {

  private readonly siteKey: string;

  constructor() {
    this.siteKey = RecaptchaKeyJson.siteKey;
  }

  getSiteKey(): string {
    return this.siteKey;
  }
}
