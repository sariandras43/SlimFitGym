import { Component } from '@angular/core';
import { TrainingModel } from '../../../Models/training.model';
import { TrainingService } from '../../../Services/training.service';
import { ButtonLoaderComponent } from '../../button-loader/button-loader.component';
import { FormsModule } from '@angular/forms';
import { RoomModel } from '../../../Models/room.model';
import { RoomService } from '../../../Services/room.service';
import { CommonModule, NgClass } from '@angular/common';
import { UserService } from '../../../Services/user.service';
import { UserModel } from '../../../Models/user.model';

enum SortDirection {
  Asc = 'asc',
  Desc = 'desc',
}

type SortableProperty = keyof Pick<
  TrainingModel,
  | 'trainer'
  | 'name'
  | 'trainingStart'
  | 'trainingEnd'
  | 'freePlaces'
  | 'maxPeople'
  | 'room'
  | 'isActive'
>;
@Component({
  selector: 'app-my-trainings-cms',
  imports: [FormsModule, ButtonLoaderComponent, NgClass, CommonModule],
  templateUrl: './my-trainings-cms.component.html',
  styleUrl: './my-trainings-cms.component.scss',
})
export class MyTrainingsCMSComponent {
  save() {
    if (!this.selectedTraining) return;
    if (this.isSubmitting || !this.selectedTraining) return;

    this.formSubmitted = true;

    this.isSubmitting = true;

    if (this.selectedTraining.id === -1) {
      this.trainingService.saveTraining(this.selectedTraining).subscribe({
        next: (updated) => {
          this.trainings.unshift({
            ...updated,
            freePlaces: updated.maxPeople,
            room: this.rooms?.find((r) => r.id == updated.roomId)?.name || '',
          });
          this.selectedTraining = undefined;
          this.isSubmitting = false;
          this.updateDisplayTrainings();
        },
        error: (err) => {
          console.error(err);
          this.isSubmitting = false;
          this.bottomError =
            err.error?.message || 'Hiba történt mentés közben.';
        },
      });
      return;
    }

    if (!this.originalTraining) {
      this.isSubmitting = false;
      this.bottomError = 'Nem található az eredeti edzés.';
      return;
    }

    const payload: Partial<TrainingModel> = { id: this.originalTraining.id };

    if (this.selectedTraining.name !== this.originalTraining.name)
      payload.name = this.selectedTraining.name;

    if (this.selectedTraining.roomId !== this.originalTraining.roomId)
      payload.roomId = this.selectedTraining.roomId;
    if (this.selectedTraining.maxPeople !== this.originalTraining.maxPeople)
      payload.maxPeople = this.selectedTraining.maxPeople;

    if (
      this.selectedTraining?.trainingStart !==
      this.originalTraining?.trainingStart
    ) {
      payload.trainingStart = this.selectedTraining.trainingStart;
    }
    if (
      this.selectedTraining?.trainingEnd !== this.originalTraining?.trainingEnd
    ) {
      payload.trainingEnd = this.selectedTraining.trainingEnd;
    }
    if (Object.keys(payload).length === 1) {
      this.selectedTraining = undefined;
      this.isSubmitting = false;
      return;
    }

    this.trainingService.saveTraining(payload).subscribe({
      next: (updated) => {
        const index = this.trainings.findIndex((p) => p.id === updated.id);
        if (index > -1) {
          this.trainings[index] = {
            ...updated,
            freePlaces: updated.maxPeople,
            room: this.rooms?.find((r) => r.id == updated.roomId)?.name || '',
          };
        } else {
          this.trainings.unshift({
            ...updated,
            freePlaces: updated.maxPeople,
            room: this.rooms?.find((r) => r.id == updated.roomId)?.name || '',
          });
        }
        this.selectedTraining = undefined;
        this.isSubmitting = false;
        this.updateDisplayTrainings();
      },
      error: (err) => {
        console.error(err);
        this.isSubmitting = false;
        this.bottomError = err.error?.message || 'Hiba mentés közben.';
      },
    });
  }
  updateDateTime(type: 'start' | 'end', field: 'date' | 'time', event: Event) {
    if (
      !this.selectedTraining ||
      !this.selectedTraining.trainingStart ||
      !this.selectedTraining.trainingEnd
    )
      return;

    const input = event.target as HTMLInputElement;
    const currentDate =
      type === 'start'
        ? new Date(this.selectedTraining.trainingStart)
        : new Date(this.selectedTraining.trainingEnd);

    if (field === 'date') {
      const [year, month, day] = input.value.split('-').map(Number);
      currentDate.setFullYear(year, month - 1, day);
    } else {
      const [hours, minutes] = input.value.split(':').map(Number);
      currentDate.setHours(hours, minutes);
    }

    if (type === 'start') {
      this.selectedTraining.trainingStart = new Date(currentDate);

      if (this.selectedTraining.trainingEnd < currentDate) {
        this.selectedTraining.trainingEnd = new Date(
          currentDate.getTime() + 3600000
        );
      }
    } else {
      this.selectedTraining.trainingEnd = new Date(currentDate);
    }

    this.validateDates();
  }

  private validateDates() {
    if (
      !this.selectedTraining ||
      !this.selectedTraining.trainingStart ||
      !this.selectedTraining.trainingEnd
    )
      return;

    const now = new Date();
    const start = this.selectedTraining.trainingStart;
    const end = this.selectedTraining.trainingEnd;

    this.bottomError = null;

    if (start < now) {
      this.bottomError = 'A kezdő dátum nem lehet a múltban!';
    }

    if (end < start) {
      this.bottomError = 'A befejezési dátum nem lehet korábbi mint a kezdő!';
      this.selectedTraining.trainingEnd = new Date(start.getTime() + 3600000);
    }
  }
  trainings: TrainingModel[] = [];
  trainerApplicants: {
    accountId: number;
    id: number;
    acceptedAt: Date | null;
  }[] = [];
  displayTrainings: TrainingModel[] = [];
  originalTraining: TrainingModel | undefined;
  selectedTraining: Partial<TrainingModel> | undefined;

  isSubmitting = false;
  formSubmitted = false;
  nameError = false;
  maxPeopleError = false;
  bottomError: string | null = null;
  minStartDate = new Date().toISOString().slice(0, 16);
  maxStartDate = new Date(new Date().setFullYear(new Date().getFullYear() + 4))
    .toISOString()
    .slice(0, 16);
  searchTerm: string = '';
  deletingTrainingId: number | null = null;
  loadingTrainingId: number | null = null;
  errorMsg: string = '';
  rooms: RoomModel[] | undefined;
  user: UserModel | undefined;
  sortState: { property: SortableProperty | null; direction: SortDirection } = {
    property: null,
    direction: SortDirection.Asc,
  };

  constructor(
    private trainingService: TrainingService,
    private roomService: RoomService,
    private userService: UserService
  ) {
    this.loadUser();
    this.loadTrainings();
    this.loadRooms();
  }
  loadUser() {
    this.userService.loggedInUser$.subscribe((usr) => {
      this.user = usr;
    });
  }
  validateDate(event: Event, type: 'start' | 'end') {
    if (
      !this.selectedTraining?.trainingStart ||
      !this.selectedTraining?.trainingEnd
    )
      return;
    const input = event.target as HTMLInputElement;
    const value = input.value;

    const currentYear = new Date().getFullYear();
    const year = Number(value.slice(0, 4));
    if (year < currentYear || year > currentYear + 10) {
      input.setCustomValidity('Érvénytelen évszám');
      this.bottomError = `Évszámnak ${currentYear}-${
        currentYear + 10
      } között kell lennie`;
      return;
    }

    if (
      type === 'start' &&
      this.selectedTraining.trainingEnd < this.selectedTraining.trainingStart
    ) {
      this.selectedTraining.trainingEnd = new Date(
        this.selectedTraining.trainingStart.getTime() + 3600000
      );
    }

    input.setCustomValidity('');
    this.bottomError = null;
  }
  handleDateChange(type: 'start' | 'end', event: string) {
    if (!this.selectedTraining) return;
    const date = new Date(event);
    if (type === 'start') {
      this.selectedTraining.trainingStart = date;
    } else {
      this.selectedTraining.trainingEnd = date;
    }
  }
  loadRooms() {
    this.roomService.rooms$.subscribe((r) => (this.rooms = r));
  }
  displayDate(training: TrainingModel) {
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

  private loadTrainings() {
    this.trainingService.getMyTrainings().subscribe({
      next: (trainings) => {
        this.trainings = trainings || [];
        this.updateDisplayTrainings();
      },
      error: (err) => console.error('Failed to load trainings:', err),
    });
  }

  search(event: Event) {
    const input = event.target as HTMLInputElement;
    this.searchTerm = input.value.toLowerCase();
    this.updateDisplayTrainings();
  }

  sortBy(property: SortableProperty) {
    if (this.sortState.property === property) {
      this.sortState.direction =
        this.sortState.direction === SortDirection.Asc
          ? SortDirection.Desc
          : SortDirection.Asc;
    } else {
      this.sortState = { property, direction: SortDirection.Asc };
    }
    this.updateDisplayTrainings();
  }

  private updateDisplayTrainings() {
    let filtered = this.trainings.filter(
      (s) =>
        s.trainer?.toLocaleLowerCase().includes(this.searchTerm) ||
        s.name?.toLocaleLowerCase().includes(this.searchTerm) ||
        s.room?.toLocaleLowerCase().includes(this.searchTerm) ||
        s.maxPeople?.toString().includes(this.searchTerm) ||
        s.freePlaces?.toString().includes(this.searchTerm)
    );

    if (this.sortState.property) {
      filtered = this.sorttrainings(
        filtered,
        this.sortState.property,
        this.sortState.direction
      );
    }

    this.displayTrainings = filtered;
  }

  private sorttrainings(
    trainings: TrainingModel[],
    property: SortableProperty,
    direction: SortDirection
  ) {
    return [...trainings].sort((a, b) => {
      const aVal = a[property];
      const bVal = b[property];

      if (typeof aVal === 'number' && typeof bVal === 'number') {
        return direction === SortDirection.Asc ? aVal - bVal : bVal - aVal;
      }
      return (
        String(aVal).localeCompare(String(bVal)) *
        (direction === SortDirection.Asc ? 1 : -1)
      );
    });
  }

  delete(training: TrainingModel) {
    if (this.deletingTrainingId) return;
    this.deletingTrainingId = training.id;
    this.trainingService.deleteTraining(training).subscribe({
      next: () => {
        this.trainings = this.trainings.filter(
          (t) => t.id != this.deletingTrainingId
        );
        this.deletingTrainingId = null;

        this.updateDisplayTrainings();
      },
      error: (err) => {
        this.errorMsg = err.error?.message || 'Hiba történt törlés közben.';

        this.deletingTrainingId = null;
      },
    });
  }

  getSortIndicator(property: SortableProperty): string {
    if (this.sortState.property !== property) return '';
    return this.sortState.direction === SortDirection.Asc ? '⬆' : '⬇';
  }

  modalOpen(training?: TrainingModel) {
    this.bottomError = null;
    if (training) {
      const original = this.trainings.find((r) => r.id === training.id);
      if (original) {
        this.originalTraining = {
          ...original,
        };
        this.selectedTraining = {
          ...original,
        };
      }
    } else {
      this.originalTraining = undefined;
      this.selectedTraining = {
        id: -1,
        name: '',
        freePlaces: 0,
        maxPeople: 0,
        roomId: 0,
        room: '',
        trainerId: this.user?.id,
        trainer: '',
        trainingStart: new Date(Date.now()),
        trainingEnd: new Date(Date.now() + 3600000),
      };
    }
  }
}
