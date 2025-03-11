import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  apiUrl = 'http://localhost:5278/api';
  constructor() { }
}
