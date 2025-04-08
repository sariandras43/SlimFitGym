import { Injectable } from '@angular/core';
import { ConfigService } from './config.service';
import { UserService } from './user.service';
import { UserModel } from '../Models/user.model';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class TrainerApplicationService {
  loggedInUser: UserModel | undefined;
  constructor(
    private http: HttpClient,
    private config: ConfigService,
    private userService: UserService
  ) {
    userService.loggedInUser$.subscribe((usr) => (this.loggedInUser = usr));
  }

  getTrainerApplicants(): Observable<
    { accountId: number; id: number; acceptedAt: Date | null }[]
  > {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.loggedInUser?.token}`
    );
    return this.http.get<
      { accountId: number; id: number; acceptedAt: Date | null }[]
    >(`${this.config.apiUrl}/applicants`, {
      headers,
    });
  }
  deleteApplicant(applicationId: number): Observable<UserModel> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.loggedInUser?.token}`
    );
    return this.http.delete<UserModel>(
      `${this.config.apiUrl}/applicants/reject/${applicationId}`,
      {
        headers,
      }
    );
  }
  acceptApplicant(applicationId: number): Observable<UserModel> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.loggedInUser?.token}`
    );
    return this.http.post<UserModel>(
      `${this.config.apiUrl}/applicants/accept/${applicationId}`,{},
      {
        headers,
      }
    );
  }
  applyForTrainer(): Observable<{id: number, accountId: number}> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.loggedInUser?.token}`
    );
    return this.http.post<{id: number, accountId: number}>(
      `${this.config.apiUrl}/applicants/${this.loggedInUser?.id}`,{},
      {
        headers,
      }
    );
  }
}
