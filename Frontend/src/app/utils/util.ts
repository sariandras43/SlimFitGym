import { TrainingModel } from "../Models/training.model";

export default class Utils {
    static displayDate(training: TrainingModel): string {
        const { trainingStart, trainingEnd } = training;
        if (trainingStart) {
          const startString = `${trainingStart.getFullYear()}.${trainingStart.getDate()}.${trainingStart.getDay()} ${trainingStart.getHours()}:${trainingStart.getHours()} - `;
          if (trainingStart.getDay() == trainingEnd.getDay()) {
            return (
              startString + `${trainingEnd.getHours()}:${trainingEnd.getMinutes()}`
            );
          }
          return (
            startString +
            `${trainingEnd.getFullYear()}.${trainingEnd.getDate()}.${trainingEnd.getDay()} ${trainingEnd.getHours()}:${trainingEnd.getHours()}`
          );
        } else if (trainingEnd) {
          return `${trainingEnd.getFullYear()}.${trainingEnd.getDate()}.${trainingEnd.getDay()} ${trainingEnd.getHours()}:${trainingEnd.getHours()}`;
        }
        return '';
      }
}