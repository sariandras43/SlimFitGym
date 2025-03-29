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
  getAllUsers(): Observable<UserModel[]> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.loggedInUserSubject.getValue()?.token}`
    );
    return this.http.get<UserModel[]>(
      `${this.config.apiUrl}/auth/accounts/all`, {headers}
    );
  }
  private loggedInUserSubject = new BehaviorSubject<UserModel | undefined>(
    undefined
  );
  loggedInUser$ = this.loggedInUserSubject.asObservable();
  private loggedInUserPassSubject = new BehaviorSubject<PassModel | undefined>(
    undefined
  );
  loggedInUserPass$ = this.loggedInUserPassSubject.asObservable();

  constructor(private config: ConfigService, private http: HttpClient) {
    this.checkUser();
  }

  login(
    email: string,
    password: string,
    rememberMe: boolean
  ): Observable<boolean> {
    return this.http
      .post<UserModel>(`${this.config.apiUrl}/auth/login`, {
        email,
        password,
        rememberMe,
      })
      .pipe(
        map((response: UserModel) => {
          this.loggedInUserSubject.next(response);

          const storage = rememberMe ? localStorage : sessionStorage;
          storage.setItem('loggedInUser', JSON.stringify(response));
          this.getPass();
          return true;
        })
      );
  }

  logout() {
    this.loggedInUserSubject.next(undefined);
    localStorage.removeItem('loggedInUser');
    sessionStorage.removeItem('loggedInUser');
    localStorage.removeItem('userPass');
    sessionStorage.removeItem('userPass');
  }
  register(
    email: string,
    name: string,
    phone: string,
    password: string,
    rememberMe: boolean
  ): Observable<boolean> {
    return this.http
      .post<UserModel>(`${this.config.apiUrl}/auth/register`, {
        name,
        email,
        phone,
        password,
        rememberMe,
      })
      .pipe(
        map((response: UserModel) => {
          this.loggedInUserSubject.next(response);
          localStorage.setItem('loggedInUser', JSON.stringify(response));
          this.getPass();
          return true;
        })
      );
  }

  updateUser(user: UserModel): Observable<boolean> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.loggedInUserSubject.getValue()?.token}`
    );
    return this.http
      .put<UserModel>(`${this.config.apiUrl}/auth/modify/${user.id}`, user, {
        headers,
      })
      .pipe(
        map((response: UserModel) => {
          response.token = this.loggedInUserSubject.getValue()?.token;
          response.validTo = this.loggedInUserSubject.getValue()?.validTo;
          this.loggedInUserSubject.next(response);
          localStorage.setItem('loggedInUser', JSON.stringify(response));
          this.getPass();
          return true;
        })
      );
  }

  checkUser() {
    const user =
      localStorage.getItem('loggedInUser') ||
      sessionStorage.getItem('loggedInUser');
    if (user) {
      const parsedUser: UserModel = JSON.parse(user);

      if (
        parsedUser.validTo &&
        new Date().getTime() > Date.parse(parsedUser.validTo)
      ) {
        this.logout();
        return;
      }

      this.loggedInUserSubject.next(parsedUser);
    }
  }

  deleteUser(user: UserModel): Observable<UserModel> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.loggedInUserSubject.getValue()?.token}`
    );
    return this.http.delete<UserModel>(
      `${this.config.apiUrl}/auth/delete/${user.id}`,
      {
        headers,
      }
    );
  }

  getPass(): void {
    let storage = localStorage;
    let user = localStorage.getItem('loggedInUser');
    if (!user) {
      user = sessionStorage.getItem('loggedInUser');
      storage = sessionStorage;
    }
    if (!user) {
      console.warn('No logged-in user found.');
      return;
    }

    const loggedInUser: UserModel = JSON.parse(user);
    if (!loggedInUser?.id || !loggedInUser?.token) {
      console.warn('Invalid user data.');
      return;
    }

    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${loggedInUser.token}`
    );

    this.http
      .get<PassModel>(
        `${this.config.apiUrl}/passes/accounts/${loggedInUser.id}/latest`,
        { headers }
      )
      .subscribe({
        next: (response) => {
          this.loggedInUserPassSubject.next(response);
          storage.setItem('userPass', JSON.stringify(response));
        },
        error: (error) => {
          console.error(
            'Failed to fetch pass:',
            error.error?.message || error.message
          );
        },
      });
  }
}
