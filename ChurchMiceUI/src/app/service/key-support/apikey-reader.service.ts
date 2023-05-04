import { Injectable } from '@angular/core';
import { IApikeyReaderService } from './apikey-reader.service.interface';
import ApiKeyJson from '../assets/apiKey.json';

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
