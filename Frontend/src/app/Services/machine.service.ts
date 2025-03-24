import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { MachineModel } from '../Models/machine.model';
import { ConfigService } from './config.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserService } from './user.service';
import { UserModel } from '../Models/user.model';

@Injectable({
  providedIn: 'root',
})
export class MachineService {
  private allMachinesSubject = new BehaviorSubject<MachineModel[] | undefined>(
    undefined
  );
  allMachines$ = this.allMachinesSubject.asObservable();
  loggedInUser: UserModel|undefined;

  constructor(private config: ConfigService, private http: HttpClient, userService: UserService) {
    const machines = localStorage.getItem('machines');
    if (machines) {
      this.allMachinesSubject.next(JSON.parse(machines));
    }
    this.getMachines();
    userService.loggedInUser$.subscribe(s=> {console.log(s),this.loggedInUser = s})
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

  saveMachine(machine: MachineModel) : Observable<MachineModel> {
    const headers = new HttpHeaders().set(
          'Authorization',
          `Bearer ${this.loggedInUser?.token}`
        );
    if(machine.id == -1)
    {
      return this.http.post<MachineModel>(`${this.config.apiUrl}/machines`, {name: machine.name, description:machine.description, images:machine.imageUrls}, {headers});
    }
    else{
      return this.http.put<MachineModel>(`${this.config.apiUrl}/machines/${machine.id}`, {id:machine.id ,name: machine.name, description:machine.description, images:machine.imageUrls}, {headers} );

    }
  }
}
