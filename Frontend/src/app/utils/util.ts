import { TrainingModel } from '../Models/training.model';

export default class Utils {
  static displayDate(training: TrainingModel): string {
    const { trainingStart, trainingEnd } = training;

    const pad = (num: number): string => num.toString().padStart(2, '0');

    if (trainingStart) {
      const startYear = trainingStart.getFullYear();
      const startMonth = pad(trainingStart.getMonth() + 1);
      const startDay = pad(trainingStart.getDate());
      const startHours = pad(trainingStart.getHours());
      const startMinutes = pad(trainingStart.getMinutes());
      let result = `${startYear}.${startMonth}.${startDay} ${startHours}:${startMinutes}`;

      if (trainingEnd) {
        const sameDay =
          trainingStart.getFullYear() === trainingEnd.getFullYear() &&
          trainingStart.getMonth() === trainingEnd.getMonth() &&
          trainingStart.getDate() === trainingEnd.getDate();

        if (sameDay) {
          result += ` - ${pad(trainingEnd.getHours())}:${pad(
            trainingEnd.getMinutes()
          )}`;
        } else {
          const endYear = trainingEnd.getFullYear();
          const endMonth = pad(trainingEnd.getMonth() + 1);
          const endDay = pad(trainingEnd.getDate());
          const endHours = pad(trainingEnd.getHours());
          const endMinutes = pad(trainingEnd.getMinutes());
          result += ` - ${endYear}.${endMonth}.${endDay} ${endHours}:${endMinutes}`;
        }
      }
      return result;
    } else if (trainingEnd) {
      const endYear = trainingEnd.getFullYear();
      const endMonth = pad(trainingEnd.getMonth() + 1);
      const endDay = pad(trainingEnd.getDate());
      const endHours = pad(trainingEnd.getHours());
      const endMinutes = pad(trainingEnd.getMinutes());
      return `${endYear}.${endMonth}.${endDay} ${endHours}:${endMinutes}`;
    }
    return '';
  }
}
