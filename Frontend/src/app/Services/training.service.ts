import { Injectable } from '@angular/core';
import { ConfigService } from './config.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TrainingModel } from '../Models/training.model';
import { BehaviorSubject, map, Observable, throwError } from 'rxjs';
import { UserService } from './user.service';
import { UserModel } from '../Models/user.model';

@Injectable({
  providedIn: 'root',
})
export class TrainingService {
  private allTrainingsSubject = new BehaviorSubject<
    TrainingModel[] | undefined
  >(undefined);
  private subscribedTrainingsSubject = new BehaviorSubject<
    TrainingModel[] | undefined
  >(undefined);
  allTrainings$ = this.allTrainingsSubject.asObservable();
  subscribedTrainings$ = this.subscribedTrainingsSubject.asObservable();
  user: UserModel |undefined; 
  constructor(
    private config: ConfigService,
    private http: HttpClient,
    private userService: UserService
  ) {
    const trainings = localStorage.getItem('trainings');
    if (trainings) {
      const parsedTrainings = JSON.parse(trainings);
      this.parseDateTime(parsedTrainings);
      this.allTrainingsSubject.next(this.subscribedOrDefault(parsedTrainings));
    }


    this.getTrainings();
    userService.loggedInUser$.subscribe((usr) => {
      if (usr) 
        this.user = usr;
        this.getSubscribedTrainings();
    });
  }
  parseDateTime(trainings: TrainingModel[]) {
    return trainings.map((d) => {
      d.trainingStart = new Date(d.trainingStart);
      d.trainingEnd = new Date(d.trainingEnd);
    });
  }
  getSubscribedTrainings() {
    if(!this.user) return;
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.user.token}`
    );
    this.http
      .get<TrainingModel[]>(
        `${this.config.apiUrl}/trainings/account/${this.user.id}`,
        { headers }
      )
      .subscribe({
        next: (response: TrainingModel[]) => {
          this.parseDateTime(response);

          this.subscribedTrainingsSubject.next(response);
          this.allTrainingsSubject.next(this.subscribedOrDefault(this.allTrainingsSubject.value));
        },
        error: (error) => {
          console.log(error.error.message ?? error.message);
        },
      });
  }

  getTrainings() {
    this.http
      .get<TrainingModel[]>(`${this.config.apiUrl}/trainings`)
      .subscribe({
        next: (response: TrainingModel[]) => {
          const parsedTraining = response;
          parsedTraining.map((d) => {
            d.trainingStart = new Date(d.trainingStart);
            d.trainingEnd = new Date(d.trainingEnd);
          });

          this.allTrainingsSubject.next(this.subscribedOrDefault(parsedTraining));
          localStorage.setItem('trainings', JSON.stringify(response));
        },
        error: (error) => {
          console.log(error.error.message ?? error.message);
        },
      });
  }
  getTrainingsInRoom(id: number): Observable<TrainingModel[]> {
    return this.http
      .get<TrainingModel[]>(`${this.config.apiUrl}/trainings/room/${id}`)
      .pipe(
        map((response) => {
          const appliedTrainings = this.subscribedTrainingsSubject.value
          return response.map((d) => ({
            ...d,
            trainingStart: new Date(d.trainingStart),
            trainingEnd: new Date(d.trainingEnd),
            userApplied: !!appliedTrainings?.some(t=> d.id == t.id)
          }));
        })
      );
  }
  subscribeToTraining(trainingId:number) : Observable<Boolean>{
    if(!this.user) return throwError(() => new Error('Nincs bejelentkezett felhasználó'));
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.user.token}`
    );
    return this.http
      .post<{trainingId: number, accountId:number}>(
        `${this.config.apiUrl}/trainings/signup`,{trainingId, accountId: this.user.id},
        { headers }
      ).pipe(
        map((response: {trainingId: number, accountId:number}) => {
          const editedTraining = this.allTrainingsSubject.value?.find(t=> t.id == trainingId)
          if(editedTraining)
            {
              
              editedTraining.userApplied = true;
              editedTraining.freePlaces--;
              this.subscribedTrainingsSubject.value?.push(editedTraining);
              return true;
            }
            return false;
        })
      );

  }
  unsubscribeFromTraining(trainingId:number) : Observable<Boolean>{
    if(!this.user) return throwError(() => new Error('Nincs bejelentkezett felhasználó'));
    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${this.user.token}`
    );
    return this.http
      .post<{trainingId: number, accountId:number}>(
        `${this.config.apiUrl}/trainings/signout`,{trainingId, accountId: this.user.id},
        { headers }
      ).pipe(
        map((response: {trainingId: number, accountId:number}) => {
          const editedTraining = this.allTrainingsSubject.value?.find(t=> t.id == trainingId)
          if(editedTraining)
          {

            editedTraining.userApplied = false;
            this.subscribedTrainingsSubject.next(this.subscribedTrainingsSubject.value?.filter(st=> st.id != trainingId))

            editedTraining.freePlaces++;
            return true;
          }
          return false;

        })
      );
  }
  subscribedOrDefault(training: TrainingModel[] | undefined){
    const subscribedTrainings = this.subscribedTrainingsSubject.value
    if(!subscribedTrainings) return training;
    return training?.map(t=>{
      return {...t,userApplied: subscribedTrainings.some(st=> t.id == st.id )};  
    })

  }
}
