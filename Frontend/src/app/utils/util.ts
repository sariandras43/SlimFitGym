import { TrainingModel } from "../Models/training.model";

export default class Utils {
    static displayDate(training: TrainingModel): string {
        const { trainingStart, trainingEnd } = training;
        if (trainingStart) {
          const startString = `${trainingStart.getFullYear()}.${trainingStart.getMonth()}.${trainingStart.getDay()} ${trainingStart.getHours()}:${trainingStart.getMinutes()} - `;
          if (trainingStart.getDay() == trainingEnd.getDay()) {
            return (
              startString + `${trainingEnd.getHours()}:${trainingEnd.getMinutes()}`
            );
          }
          return (
            startString +
            `${trainingEnd.getFullYear()}.${trainingEnd.getMonth()}.${trainingEnd.getDay()} ${trainingEnd.getHours()}:${trainingEnd.getMinutes()}`
          );
        } else if (trainingEnd) {
          return `${trainingEnd.getFullYear()}.${trainingEnd.getMonth()}.${trainingEnd.getDay()} ${trainingEnd.getHours()}:${trainingEnd.getMinutes()}`;
        }
        return '';
      }
}