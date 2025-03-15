import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { MachineModel } from '../Models/machine.model';
import { ConfigService } from './config.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class MachineService {
  private allMachinesSubject = new BehaviorSubject<MachineModel[] | undefined>(
    undefined
  );
  allMachines$ = this.allMachinesSubject.asObservable();

  constructor(private config: ConfigService, private http: HttpClient) {
    const machines = localStorage.getItem('machines');
    if (machines) {
      this.allMachinesSubject.next(JSON.parse(machines));
    }
    this.getMachines();
  }

  getMachines() {
    this.http.get<MachineModel[]>(`${this.config.apiUrl}/machines`).subscribe({
      next: (response: MachineModel[]) => {
        this.allMachinesSubject.next(response);
        localStorage.setItem('machines', JSON.stringify(response));
      },
      error: (error) => {
        console.log(error.error.message ?? error.message);
      },
    });
  }
}
