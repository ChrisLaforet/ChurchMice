import { Injectable } from '@angular/core';
import { IApiKeyReaderService } from '@service/index';
import ApiKeyJson from '@assets/api-key.json';

@Injectable({
  providedIn: 'root'
})
export class ApiKeyReaderService implements IApiKeyReaderService {

  private readonly apiKey: string;

  constructor() {
    this.apiKey = ApiKeyJson.apiKey;
  }

  getApiKey(): string {
    return this.apiKey;
  }
}
