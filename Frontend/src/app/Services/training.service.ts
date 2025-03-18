import { Injectable } from '@angular/core';
import { ConfigService } from './config.service';
import { HttpClient } from '@angular/common/http';
import { TrainingModel } from '../Models/training.model';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TrainingService {
  private allTrainingsSubject = new BehaviorSubject<TrainingModel[] | undefined>(
    undefined
  );
  allTrainings$ = this.allTrainingsSubject.asObservable();

  constructor(private config: ConfigService, private http: HttpClient) {
    const trainings = localStorage.getItem('trainings');
    if (trainings) {
      this.allTrainingsSubject.next(JSON.parse(trainings));
    }
    this.getTrainings();
  }

  getTrainings() {
    this.http.get<TrainingModel[]>(`${this.config.apiUrl}/trainings`).subscribe({
      next: (response: TrainingModel[]) => {
        const parsedTraining = response;
        parsedTraining.map(d=>{
          d.trainingStart = new Date(d.trainingStart)
          d.trainingEnd = new Date(d.trainingEnd)
        })


        this.allTrainingsSubject.next(response);
        localStorage.setItem('trainings', JSON.stringify(response));
      },
      error: (error) => {
        console.log(error.error.message ?? error.message);
      },
    });
  }
}
