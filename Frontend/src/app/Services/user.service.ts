import { Injectable } from '@angular/core';
import { UserModel } from '../Models/user.model';
import { ConfigService } from './config.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { PassModel } from '../Models/pass.model';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private loggedInUserSubject = new BehaviorSubject<UserModel | undefined>(
    undefined
  );
  loggedInUser$ = this.loggedInUserSubject.asObservable();
  private loggedInUserPassSubject = new BehaviorSubject<PassModel | undefined>(
    undefined
  );
  loggedInUserPass$ = this.loggedInUserPassSubject.asObservable();

  constructor(
    private config: ConfigService,
    private http: HttpClient,
    private cookieService: CookieService
  ) {
    this.checkUser();
  }

  private getAuthHeaders(): HttpHeaders {
    const token = this.loggedInUserSubject.getValue()?.token;
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  getAllUsers(): Observable<UserModel[]> {
    return this.http.get<UserModel[]>(
      `${this.config.apiUrl}/auth/accounts/all`,
      { headers: this.getAuthHeaders() }
    );
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
          this.handleAuthResponse(response, rememberMe);
          return true;
        })
      );
  }

  logout() {
    this.loggedInUserSubject.next(undefined);
    this.loggedInUserPassSubject.next(undefined);
    this.cookieService.delete('authData', '/');
    this.cookieService.delete('passData', '/');
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
          this.handleAuthResponse(response, rememberMe);
          return true;
        })
      );
  }

  updateUser(user: UserModel): Observable<boolean> {
    return this.http
      .put<UserModel>(`${this.config.apiUrl}/auth/modify/${user.id}`, user, {
        headers: this.getAuthHeaders(),
      })
      .pipe(
        map((updatedUser: UserModel) => {
          const currentUser = this.loggedInUserSubject.getValue();
          const mergedUser = { ...currentUser, ...updatedUser, token: currentUser?.token, validTo: currentUser?.validTo };
          this.loggedInUserSubject.next(mergedUser);
          this.cookieService.set('authData', JSON.stringify(mergedUser), {
            secure: true,
            sameSite: 'Strict',
            path: '/',
          });
          return true;
        })
      );
  }

  deleteUser(user: UserModel): Observable<UserModel> {
    return this.http.delete<UserModel>(
      `${this.config.apiUrl}/auth/delete/${user.id}`,
      { headers: this.getAuthHeaders() }
    );
  }

  getPass(): void {
    const userId = this.loggedInUserSubject.getValue()?.id;
    if (!userId) {
      console.warn('No user ID available for pass retrieval');
      return;
    }

    this.http
      .get<PassModel>(
        `${this.config.apiUrl}/passes/accounts/${userId}/latest`,
        { headers: this.getAuthHeaders() }
      )
      .subscribe({
        next: (pass) => {
          this.loggedInUserPassSubject.next(pass);
          this.cookieService.set('passData', JSON.stringify(pass), {
            secure: true,
            sameSite: 'Strict',
            path: '/',
          });
        },
        error: (error) => {
          console.error(
            'Failed to fetch pass:',
            error.error?.message || error.message
          );
        },
      });
  }

  private checkUser() {
    const authCookie = this.cookieService.get('authData');
    if (!authCookie) return;

    try {
      const user: UserModel = JSON.parse(authCookie);

      if (user.validTo && new Date(Date.now()) > new Date(user.validTo)) {
        this.logout();
        return;
      }
      this.loggedInUserSubject.next(user);
      this.http.get<UserModel>(`${this.config.apiUrl}/auth/me`, {headers: this.getAuthHeaders()}).subscribe({
        next: (response) => {
          this.loggedInUserSubject.next({...response, token: this.loggedInUserSubject.value?.token, validTo: this.loggedInUserPassSubject.value?.validTo});
        },
        error: () => {
          this.loggedInUserSubject.next(undefined);
        },
      });

      this.restorePassFromCookie();
    } catch (error) {
      console.error('Error parsing auth cookie:', error);
      this.logout();
    }
  }

  private handleAuthResponse(response: UserModel, rememberMe: boolean) {
    const cookieOptions = {
      expires: rememberMe
        ? new Date(Date.now() + 7 * 24 * 60 * 60 * 1000)
        : undefined,
      secure: true,
      sameSite: 'Strict' as const,
      path: '/',
    };

    this.loggedInUserSubject.next(response);
    this.cookieService.set('authData', JSON.stringify(response), cookieOptions);
    this.getPass();
  }

  private restorePassFromCookie() {
    const passCookie = this.cookieService.get('passData');
    if (passCookie) {
      try {
        const pass: PassModel = JSON.parse(passCookie);
        this.loggedInUserPassSubject.next(pass);
      } catch (error) {
        console.error('Error parsing pass cookie:', error);
      }
    }
  }
}
