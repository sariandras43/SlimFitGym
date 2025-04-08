import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  apiUrl = 'https://slimfitgymbackend-bdgbechedpcpaag4.westeurope-01.azurewebsites.net/api';
  constructor() { }
}
