import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { UserService } from './user.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ConfigService } from './config.service';
import { PurchaseStatisticModel } from '../Models/purchaseStatistic.model';
import { EntryStatisticModel } from '../Models/entryStatistic.model';

@Injectable({
  providedIn: 'root',
})
export class StatisticsService {
  private entriesSubject = new BehaviorSubject<
    EntryStatisticModel[] | undefined
  >(undefined);
  entries$ = this.entriesSubject.asObservable();

  private purchasesSubject = new BehaviorSubject<
    PurchaseStatisticModel[] | undefined
  >(undefined);
  purchases$ = this.purchasesSubject.asObservable();
  constructor(
    private userService: UserService,
    private config: ConfigService,
    private http: HttpClient
  ) {
    this.userService.loggedInUser$.subscribe((usr) => {
      if (usr) {
        this.getEntriesStatistic();
        this.getPurchasesStatistic();
      }
    });
  }
  getPurchasesStatistic() {
    this.http
      .get<PurchaseStatisticModel[]>(
        `${this.config.apiUrl}/statistics/purchases?year=${new Date().getFullYear()}`,
        {
          headers: this.userService.getAuthHeaders(),
        }
      )
      .subscribe({
        next: (stat) => {
          this.purchasesSubject.next(stat);
        },
        error: (error) => {
          console.error(
            'Failed to fetch statistic:',
            error.error?.message || error.message
          );
        },
      });
  }
  getEntriesStatistic() {
    this.http
      .get<EntryStatisticModel[]>(
        `${this.config.apiUrl}/statistics/entries?year=${new Date().getFullYear()}`,
        {
          headers: this.userService.getAuthHeaders(),
        }
      )
      .subscribe({
        next: (stat) => {
          this.entriesSubject.next(stat);
        },
        error: (error) => {
          console.error(
            'Failed to fetch statistic:',
            error.error?.message || error.message
          );
        },
      });
  }
}
