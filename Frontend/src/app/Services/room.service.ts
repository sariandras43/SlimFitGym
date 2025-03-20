import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { ConfigService } from './config.service';
import { HttpClient } from '@angular/common/http';
import { RoomModel } from '../Models/room.model';
import { TrainingModel } from '../Models/training.model';

@Injectable({
  providedIn: 'root',
})
export class RoomService {
  private roomsSubject = new BehaviorSubject<RoomModel[] | undefined>(
    undefined
  );
  rooms$ = this.roomsSubject.asObservable();

  constructor(private config: ConfigService, private http: HttpClient) {
    const rooms = localStorage.getItem('rooms')
    if(rooms){
      this.roomsSubject.next(JSON.parse(rooms));
    }
    this.getRooms();
  }

  getRooms() {
    this.http.get<RoomModel[]>(`${this.config.apiUrl}/rooms`).subscribe({
      next: (response: RoomModel[]) => {
        this.roomsSubject.next(response);
        localStorage.setItem('rooms', JSON.stringify(response));
      },
      error: (error) => {
        console.log(error.error.message ?? error.message);
      },
    });
  }
  getRoom(id:Number): Observable<RoomModel>{
    return this.http.get<RoomModel>(`${this.config.apiUrl}/rooms/${id}`);
  }
  getTrainingsInRoom(id: Number): Observable<TrainingModel[]>{
    return this.http.get<TrainingModel[]>(`${this.config.apiUrl}/trainings/room/${id}`).pipe(
      map((response: TrainingModel[]) => {
        const parsedTraining = response;
        parsedTraining.map(d=>{
          d.trainingStart = new Date(d.trainingStart)
          d.trainingEnd = new Date(d.trainingEnd)
        })
        return parsedTraining;
      })
    );
  }
}
