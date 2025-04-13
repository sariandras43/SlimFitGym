import { Injectable } from '@angular/core';
import { ConfigService } from './config.service';
import { HttpClient } from '@angular/common/http';
import { UserService } from './user.service';
import { BehaviorSubject } from 'rxjs';
import { PurchaseModel } from '../Models/purchase.model';

@Injectable({
  providedIn: 'root',
})
export class PurchaseService {
  private purchasesSubject = new BehaviorSubject<PurchaseModel[] | undefined>(
    undefined
  );
  purchases$ = this.purchasesSubject.asObservable();
  constructor(
    private config: ConfigService,
    private http: HttpClient,
    private userService: UserService
  ) {
    userService.loggedInUser$.subscribe(() => this.getPurchase());
  }
  getPurchase() {
    this.http
      .get<PurchaseModel[]>(
        `${this.config.apiUrl}/purchases`,
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
            'Failed to fetch purchases:',
            error.error?.message || error.message
          );
        },
      });
  }
}
