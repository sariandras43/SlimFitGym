import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { RoomModel } from '../Models/room.model';
import { ConfigService } from './config.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserService } from './user.service';
import { UserModel } from '../Models/user.model';
import { TrainingModel } from '../Models/training.model';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class RoomService {
  private roomsSubject = new BehaviorSubject<RoomModel[] | undefined>(undefined);
  rooms$ = this.roomsSubject.asObservable();
  loggedInUser: UserModel | undefined;

  constructor(
    private config: ConfigService,
    private http: HttpClient,
    private userService: UserService
  ) {
    const rooms = localStorage.getItem('rooms');
    if (rooms) {
      this.roomsSubject.next(JSON.parse(rooms));
    }
    this.getRooms();
    this.userService.loggedInUser$.subscribe(user => {
      this.loggedInUser = user;
    });
  }

  saveRoom(room: RoomModel): Observable<RoomModel> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.loggedInUser?.token}`
    );
     
    const payload = {
      ...room,
      machines: room.machines.map((m) => ({
        id: m.id,
        count: m.machineCount || 0,
      })),
    };
    if (payload.id === -1) {
      const { id, ...postRoom } = payload;
      return this.http.post<RoomModel>(
        `${this.config.apiUrl}/rooms`,
        postRoom,
        { headers }
      );
    } else {
      return this.http.put<RoomModel>(
        `${this.config.apiUrl}/rooms/${payload.id}`,
        payload,
        { headers }
      );
    }
  }

  deleteRoom(room: RoomModel): Observable<RoomModel> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.loggedInUser?.token}`
    );
    return this.http.delete<RoomModel>(
      `${this.config.apiUrl}/rooms/${room.id}`,
      { headers }
    );
  }

  getRooms() {
    this.http.get<RoomModel[]>(`${this.config.apiUrl}/rooms`).subscribe({
      next: (response) => {
        this.roomsSubject.next(response);
        localStorage.setItem('rooms', JSON.stringify(response));
      },
      error: (error) => {
        console.log(error.error.message ?? error.message);
      },
    });
  }

  getRoomsAll(): Observable<RoomModel[]> {
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.loggedInUser?.token}`
    );
    return this.http.get<RoomModel[]>(`${this.config.apiUrl}/rooms/all`, { headers });
  }

  getRoom(id: number): Observable<RoomModel> {
    return this.http.get<RoomModel>(`${this.config.apiUrl}/rooms/${id}`);
  }

  getTrainingsInRoom(id: number): Observable<TrainingModel[]> {
    return this.http.get<TrainingModel[]>(`${this.config.apiUrl}/trainings/room/${id}`).pipe(
      map((response) => {
        return response.map(d => ({
          ...d,
          trainingStart: new Date(d.trainingStart),
          trainingEnd: new Date(d.trainingEnd)
        }));
      })
    );
  }
}