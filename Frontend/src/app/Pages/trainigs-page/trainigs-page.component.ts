import { Component } from '@angular/core';
import { HeroComponent } from '../../Components/hero/hero.component';
import { TrainingModel } from '../../Models/training.model';
import { TrainingService } from '../../Services/training.service';

@Component({
  selector: 'app-trainigs-page',
  imports: [HeroComponent],
  templateUrl: './trainigs-page.component.html',
  styleUrl: './trainigs-page.component.scss',
})
export class TrainigsPageComponent {
  trainings: TrainingModel[] | undefined;
  constructor(trainingservice: TrainingService) {
    trainingservice.allTrainings$.subscribe((trainings) => {
      this.trainings = trainings;
    });
  }
  displayDate(training: TrainingModel) {
    const {trainingStart,trainingEnd } = training;
    const startString = `${trainingStart.getFullYear()}.${trainingStart.getDate()}.${trainingStart.getDay()} ${trainingStart.getHours()}:${trainingStart.getHours()} - `;
    if (trainingStart.getDay() == trainingEnd.getDay()) {
      return startString + `${trainingEnd.getHours()}:${trainingEnd.getMinutes()}`;
    }
    return startString + `${trainingEnd.getFullYear()}.${trainingEnd.getDate()}.${trainingEnd.getDay()} ${trainingEnd.getHours()}:${trainingEnd.getHours()}`;
  }
}
