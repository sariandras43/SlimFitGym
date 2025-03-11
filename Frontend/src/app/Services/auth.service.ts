import { Injectable } from '@angular/core';
import { UserModel } from '../Models/user.model';
import { ConfigService } from './config.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { PassModel } from '../Models/pass.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  loggedInUser: UserModel | undefined = undefined;
  loggedInUserPass: PassModel |undefined = undefined;

  constructor(private config: ConfigService, private http: HttpClient) {}

  login(email: string, password: string, rememberMe: boolean): Observable<boolean> {
    return this.http
      .post<UserModel>(
        `${this.config.apiUrl}/auth/login`,
        { email, password, rememberMe },
      )
      .pipe(
        map((response: UserModel) => {
          this.loggedInUser = response;
          localStorage.setItem(
            'loggedInUser',
            JSON.stringify(this.loggedInUser)
          );
          this.getPass();
          return true;
        })
      );
  }

  logout() {
    this.loggedInUser = undefined;
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

  getPass() : Observable<boolean>{
    
    const user = localStorage.getItem('loggedInUser');
    let headers : HttpHeaders | undefined ;
    if (user){
      this.loggedInUser = JSON.parse(user);
      
      headers = new HttpHeaders().set('authorization', `Bearer ${this.loggedInUser?.token}`);
    }
    return this.http.get<PassModel>(
        `${this.config.apiUrl}/passes/accounts/${this.loggedInUser?.id}/latest`,
         { headers }
      )
      .pipe(
        map((response: PassModel) => {
          
          this.loggedInUserPass = response;
          localStorage.setItem(
            'userPass',
            JSON.stringify(this.loggedInUserPass)
          );
          return true;
        })
      );
  }
}
