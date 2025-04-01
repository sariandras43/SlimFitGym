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
  templateUrl: './trainings-page.component.html',
  styleUrl: './trainings-page.component.scss',
})
export class TrainigsPageComponent {
  subscribe(training: TrainingModel) {
    this.trainingService.subscribeToTraining(training.id).subscribe({
      next: () => {},
      error: (err) => {
        console.log(err);
      },
    });
  }
  unsubscribe(training: TrainingModel) {
    this.trainingService.unsubscribeFromTraining(training.id).subscribe({
      next: () => {},
      error: (err) => {
        console.log(err);
      },
    });
  }
  trainings: TrainingModel[] = [];
  user: UserModel | undefined;

  constructor(
    private trainingService: TrainingService,
    userService: UserService
  ) {
    trainingService.allTrainings$.subscribe({
      next: (t) => {
        if (t) {
          this.trainings = t;
        }
      },
      error: (err) => {
        console.log(err);
      },
    });
    userService.loggedInUser$.subscribe((s) => {
      if (s) this.user = s;
    });
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
