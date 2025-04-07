import { Component } from '@angular/core';
import { HeroComponent } from '../../Components/hero/hero.component';
import { TrainingModel } from '../../Models/training.model';
import { TrainingService } from '../../Services/training.service';
import { combineLatest, Subscription } from 'rxjs';
import { UserService } from '../../Services/user.service';
import { UserModel } from '../../Models/user.model';
import { FooterComponent } from '../../Components/footer/footer.component';
import Util from '../../utils/util';

@Component({
  selector: 'app-trainigs-page',
  imports: [HeroComponent, FooterComponent],
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
    return Util.displayDate(training);
  }
}
