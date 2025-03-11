import { Injectable } from '@angular/core';
import { UserModel } from '../Models/user.model';
import { ConfigService } from './config.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { PassModel } from '../Models/pass.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private loggedInUserSubject = new BehaviorSubject<UserModel | undefined>(undefined);
  loggedInUser$ = this.loggedInUserSubject.asObservable();
  private loggedInUserPassSubject = new BehaviorSubject<PassModel | undefined>(undefined);
  loggedInUserPass$ = this.loggedInUserPassSubject.asObservable();

  constructor(private config: ConfigService, private http: HttpClient) {}

  login(email: string, password: string, rememberMe: boolean): Observable<boolean> {
    return this.http
      .post<UserModel>(
        `${this.config.apiUrl}/auth/login`,
        { email, password, rememberMe },
      )
      .pipe(
        map((response: UserModel) => {
          
          this.loggedInUserSubject.next(response);
          localStorage.setItem(
            'loggedInUser',
            JSON.stringify(response)
          );
          this.getPass();
          return true;
        })
      );
  }

  logout() {
    this.loggedInUserSubject.next(undefined);
    localStorage.removeItem('loggedInUser');

    this.http.post(`${this.config.apiUrl}/auth/logout)`, {}).subscribe();
  }
  // checkUser() {
  //   const user = localStorage.getItem('loggedInUser');
  //   if (user) {
  //     const headers: HttpHeaders = new HttpHeaders().set('Authorization', `${JSON.parse(user).token}`);
  //     this.http.get<UserModel>(`${this.config.apiUrl}/auth`, { headers }).subscribe({
  //       next: (response: UserModel) => {
  //         this.loggedInUser = response;
  //         localStorage.setItem('loggedInUser', JSON.stringify(this.loggedInUser));
  //       },
  //       error: () => {
  //         this.loggedInUser = undefined;
  //         localStorage.removeItem('loggedInUser');
  //       }
  //     });
  //   }
  // }

  getPass() : void{
    
    const user = localStorage.getItem('loggedInUser');
    let loggedInUser: UserModel | undefined;
    let headers : HttpHeaders | undefined ;
    if (user){
      loggedInUser = JSON.parse(user);
      
      headers = new HttpHeaders().set('authorization', `Bearer ${loggedInUser?.token}`);
    }
    this.http.get<PassModel>(
        `${this.config.apiUrl}/passes/accounts/${loggedInUser?.id}/latest`,
         { headers }
      )
      .pipe(
        map((response: PassModel) => {
          this.loggedInUserPass$
          this.loggedInUserPassSubject.next(response)
          localStorage.setItem(
            'userPass',
            JSON.stringify(response)
          );
          return true;
        })
      ).subscribe({
        next: (response) => {
          if (response) {
            const loggedInUserPass = localStorage.getItem('userPass');
            if (loggedInUserPass) {
              this.loggedInUserPassSubject.next(JSON.parse(loggedInUserPass));
            } 
          }
          
        },
        error: (error) => {
          console.log(error.error.message ?? error.message)
        }
      });
  }
}
