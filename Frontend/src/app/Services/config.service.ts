import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  apiUrl = 'http://localhost:8080/api';
  constructor() { }
}
