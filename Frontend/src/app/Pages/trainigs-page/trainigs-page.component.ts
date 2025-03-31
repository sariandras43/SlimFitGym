import { Component } from '@angular/core';
import { HeroComponent } from '../../Components/hero/hero.component';
import { TrainingModel } from '../../Models/training.model';
import { TrainingService } from '../../Services/training.service';
import { combineLatest, Subscription } from 'rxjs';
import { UserService } from '../../Services/user.service';
import { UserModel } from '../../Models/user.model';

@Component({
  selector: 'app-trainigs-page',
  imports: [HeroComponent],
  templateUrl: './trainigs-page.component.html',
  styleUrl: './trainigs-page.component.scss',
})
export class TrainigsPageComponent {
  subscribe(training: TrainingModel) {
    this.trainingService.subscribeToTraining(training.id).subscribe({
      next: () => {
        training.userApplied = true;
        training.freePlaces--;
      },
      error: (err) => {
        console.log(err);
        
      },
    });
  }
  unsubscribe(training: TrainingModel) {
    this.trainingService.unsubscribeFromTraining(training.id).subscribe({
      next: () => {
        training.freePlaces++;
        training.userApplied = false;
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
  trainings: TrainingModel[] = [];
  private subscriptions: Subscription[] = [];
  user: UserModel| undefined;

  constructor(
    private trainingService: TrainingService,
    userService: UserService
  ) {
    const combined$ = combineLatest<
      [TrainingModel[] | undefined, TrainingModel[] | undefined]
    >([trainingService.allTrainings$, trainingService.subscribedTrainings$]);

    this.subscriptions.push(
      combined$.subscribe(([allTrainings, subscTrainings]) => {
        if (!allTrainings) return;
        if (!subscTrainings) {
          this.trainings = allTrainings;
          return;
        }
        this.trainings = allTrainings.map((training) => ({
          ...training,
          userApplied: subscTrainings.some((sub) => sub.id === training.id),
        }));
      })
    );

    userService.loggedInUser$.subscribe((s) => {
      if (s) this.user = s;
    });
  }
  ngOnDestroy(): void {
    this.subscriptions.forEach((sub) => sub.unsubscribe());
  }
  displayDate(training: TrainingModel): string {
    const { trainingStart, trainingEnd } = training;
    if (!(trainingStart instanceof Date) || !(trainingEnd instanceof Date))
      return '';

    const pad = (num: number) => num.toString().padStart(2, '0');

    const startDate = `${trainingStart.getFullYear()}.${
      trainingStart.getMonth() + 1
    }.${trainingStart.getDate()}`;
    const startTime = `${pad(trainingStart.getHours())}:${pad(
      trainingStart.getMinutes()
    )}`;

    if (trainingStart.toDateString() === trainingEnd.toDateString()) {
      return `${startDate} ${startTime} - ${pad(trainingEnd.getHours())}:${pad(
        trainingEnd.getMinutes()
      )}`;
    } else {
      const endDate = `${trainingEnd.getFullYear()}.${
        trainingEnd.getMonth() + 1
      }.${trainingEnd.getDate()}`;
      const endTime = `${pad(trainingEnd.getHours())}:${pad(
        trainingEnd.getMinutes()
      )}`;
      return `${startDate} ${startTime} - ${endDate} ${endTime}`;
    }
  }
}
