import { Injectable } from '@angular/core';
import { IApikeyReaderService } from '@service/index';
import ApiKeyJson from '@assets/api-key.json';

@Injectable({
  providedIn: 'root'
})
export class ApikeyReaderService implements IApikeyReaderService {

  private readonly apiKey: string;

  constructor() {
    this.apiKey = ApiKeyJson.apiKey;
  }

  getApikey(): string {
    return this.apiKey;
  }
}
