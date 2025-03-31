import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TrainingModel } from '../../Models/training.model';
import { UserModel } from '../../Models/user.model';

@Component({
  selector: 'app-events',
  imports: [],
  templateUrl: './events.component.html',
  styleUrl: './events.component.scss'
})
export class EventsComponent {
subscribe(training: TrainingModel) {
this.subscribed.emit(training);
}
unsubscribe(training: TrainingModel) {
this.unsubscribed.emit(training)
}
  @Input() events : Array<TrainingModel> | undefined;
  @Input() user : UserModel |undefined;
  @Output() subscribed = new EventEmitter<TrainingModel>;
  @Output() unsubscribed = new EventEmitter<TrainingModel>;
  displayDate(training: TrainingModel) {
    const {trainingStart,trainingEnd } = training;
    const startString = `${trainingStart.getFullYear()}.${trainingStart.getDate()}.${trainingStart.getDay()} ${trainingStart.getHours()}:${trainingStart.getHours()} - `;
    if (trainingStart.getDay() == trainingEnd.getDay()) {
      return startString + `${trainingEnd.getHours()}:${trainingEnd.getMinutes()}`;
    }
    return startString + `${trainingEnd.getFullYear()}.${trainingEnd.getDate()}.${trainingEnd.getDay()} ${trainingEnd.getHours()}:${trainingEnd.getHours()}`;
  }
}
