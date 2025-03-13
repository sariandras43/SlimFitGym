import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { PassModel } from '../Models/pass.model';
import { ConfigService } from './config.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class PassService {
  private allPassesSubject = new BehaviorSubject<PassModel[] | undefined>(
    undefined
  );
  allPasses$ = this.allPassesSubject.asObservable();

  constructor(private config: ConfigService, private http: HttpClient) {
    const passes = localStorage.getItem('passes')
    if(passes){
      this.allPassesSubject.next(JSON.parse(passes));
    }
    this.getPasses();
  }

  getPasses() {
    this.http.get<PassModel[]>(`${this.config.apiUrl}/passes`).subscribe({
      next: (response: PassModel[]) => {
        this.allPassesSubject.next(response);
        localStorage.setItem('passes', JSON.stringify(response));
      },
      error: (error) => {
        console.log(error.error.message ?? error.message);
      },
    });
  }
}
