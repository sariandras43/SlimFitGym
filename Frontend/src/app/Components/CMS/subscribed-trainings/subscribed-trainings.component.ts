import { Component } from '@angular/core';
import { TrainingModel } from '../../../Models/training.model';
import { UserModel } from '../../../Models/user.model';
import { TrainingService } from '../../../Services/training.service';
import { UserService } from '../../../Services/user.service';
import Utils from '../../../utils/util';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-subscribed-trainings',
  imports: [FormsModule, RouterLink],
  templateUrl: './subscribed-trainings.component.html',
  styleUrl: './subscribed-trainings.component.scss',
})
export class SubscribedTrainingsComponent {
  searchValue = '';
  isLoading = false;

  search() {
    this.updateDisplayTrainings();
  }

  trainings: TrainingModel[] = [];
  displayTrainings: TrainingModel[] = [];
  user: UserModel | undefined;

  constructor(private trainingService: TrainingService) {
    trainingService.subscribedTrainings$.subscribe({
      next: (t) => {
        if (t) {
          this.trainings = t
            .filter((t) => {
              return t.trainingEnd > new Date(Date.now());

            })
            .sort((training) => training.trainingStart.getTime());
          this.updateDisplayTrainings();
        }
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  displayDate(training: TrainingModel): string {
    return Utils.displayDate(training);
  }
  updateDisplayTrainings() {
    this.displayTrainings = this.trainings.filter(
      (t) =>
        t.name.toLowerCase().includes(this.searchValue) ||
        t.trainer.toLowerCase().includes(this.searchValue)
    );
  }

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
}
