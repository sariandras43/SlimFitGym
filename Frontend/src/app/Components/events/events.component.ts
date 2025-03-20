import { Component, Input } from '@angular/core';
import { TrainingModel } from '../../Models/training.model';

@Component({
  selector: 'app-events',
  imports: [],
  templateUrl: './events.component.html',
  styleUrl: './events.component.scss'
})
export class EventsComponent {
  @Input() events : Array<TrainingModel> = Array<TrainingModel>(10)
  displayDate(training: TrainingModel) {
    const {trainingStart,trainingEnd } = training;
    const startString = `${trainingStart.getFullYear()}.${trainingStart.getDate()}.${trainingStart.getDay()} ${trainingStart.getHours()}:${trainingStart.getHours()} - `;
    if (trainingStart.getDay() == trainingEnd.getDay()) {
      return startString + `${trainingEnd.getHours()}:${trainingEnd.getMinutes()}`;
    }
    return startString + `${trainingEnd.getFullYear()}.${trainingEnd.getDate()}.${trainingEnd.getDay()} ${trainingEnd.getHours()}:${trainingEnd.getHours()}`;
  }
}
