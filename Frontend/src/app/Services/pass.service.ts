import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { PassModel } from '../Models/pass.model';
import { ConfigService } from './config.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserService } from './user.service';
import { UserModel } from '../Models/user.model';

@Injectable({
  providedIn: 'root',
})
export class PassService {
  private allPassesSubject = new BehaviorSubject<PassModel[] | undefined>(
    undefined
  );
  allPasses$ = this.allPassesSubject.asObservable();
  loggedInUser: UserModel | undefined;
  constructor(private config: ConfigService, private http: HttpClient, private userService: UserService) {
    const passes = localStorage.getItem('passes')
    if(passes){
      this.allPassesSubject.next(JSON.parse(passes));
    }
    this.getPasses();
    userService.loggedInUser$.subscribe(s=> {console.log(s),this.loggedInUser = s})
  }
  savePass(pass: PassModel) : Observable<PassModel> {
    const headers = new HttpHeaders().set(
              'Authorization',
              `Bearer ${this.loggedInUser?.token}`
            );
        if(pass.id == -1)
        {
          const {id, ...postPass} = pass; 
          return this.http.post<PassModel>(`${this.config.apiUrl}/passes`, postPass, {headers});
        }
        else{
          return this.http.put<PassModel>(`${this.config.apiUrl}/passes/${pass.id}`, pass, {headers} );
    
        }
  }

  buyPass(buy:{passId:number, accountId:number}) : Observable<{passId:number, accountId:number}> {
    const headers = new HttpHeaders().set(
              'Authorization',
              `Bearer ${this.loggedInUser?.token}`
            );
          return this.http.post<{passId:number, accountId:number}>(`${this.config.apiUrl}/purchases`, buy, {headers});
        
  }
  
  deletePass(pass: PassModel) : Observable<PassModel> {
    const headers = new HttpHeaders().set(
          'Authorization',
          `Bearer ${this.loggedInUser?.token}`
        );
        return this.http.delete<PassModel>(`${this.config.apiUrl}/passes/${pass.id}`, {headers});
      
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
  getPassesAll() : Observable<PassModel[]>{
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.loggedInUser?.token}`
    );
    return this.http.get<PassModel[]>(`${this.config.apiUrl}/passes/all`, {headers})
  }
}
