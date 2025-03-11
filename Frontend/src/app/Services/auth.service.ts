import { Injectable } from '@angular/core';
import { UserModel } from '../Models/user.model';
import { ConfigService } from './config.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  loggedInUser: UserModel | undefined = undefined;

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
}
