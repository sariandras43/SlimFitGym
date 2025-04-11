import { Injectable } from '@angular/core';
import { ConfigService } from './config.service';
import { HttpClient } from '@angular/common/http';
import { UserService } from './user.service';
import { UserModel } from '../Models/user.model';
import { Entry } from '../Models/entry.model';
import { BehaviorSubject, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class EntryService {
  private entriesSubject = new BehaviorSubject<Entry[]>([]);
  displayEntries$ = this.entriesSubject.asObservable();
  constructor(
    private config: ConfigService,
    private http: HttpClient,
    private userService: UserService
  ) {}
  getEntries(args: {
    limit: number;
    offset: number;
    orderDirection?: string;
    orderField?: string;
    accountId?: number;
  }) {
    let { limit, offset, orderDirection, orderField, accountId } = args;
    orderDirection = orderDirection || 'desc';

    let queryLink =
      `${this.config.apiUrl}/entries` +
      (accountId ? `/${accountId}` : '') +
      `?limit=${limit}&offset=${offset}&orderDirection=${orderDirection}` +
      (orderField ? `&orderField=${orderField}` : '');

    this.http
      .get<Entry[]>(queryLink, { headers: this.userService.getAuthHeaders() })
      .subscribe({
        next: (response: Entry[]) => {
          this.entriesSubject.next(
            response.map((r) => {
              return { ...r, entryDate: new Date(r.entryDate) };
            })
          );
        },
        error: (error) => {
          console.log(error.error.message ?? error.message);
        },
      });
  }
  enter(entry: Entry) {
    return this.http
      .post<Entry>(
        `${this.config.apiUrl}/entries/${entry.accountId}`,
        {},
        { headers: this.userService.getAuthHeaders() }
      )
      .pipe(
        map((res: Entry) => {
          if (!res.name) {
            res.name = entry.name;
          }
          if (res.entryDate) {
            res.entryDate = new Date(res.entryDate);
          }
          this.entriesSubject.next([res, ...this.entriesSubject.value]);
          return true;
        })
      );
  }
}
