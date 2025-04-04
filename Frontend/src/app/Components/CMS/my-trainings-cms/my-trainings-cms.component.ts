import { Component } from '@angular/core';
import { TrainingModel } from '../../../Models/training.model';
import { TrainingService } from '../../../Services/training.service';
import { ButtonLoaderComponent } from "../../button-loader/button-loader.component";



enum SortDirection {
  Asc = 'asc',
  Desc = 'desc',
}

type SortableProperty = keyof Pick<
  TrainingModel,
  'trainer' | 'name' | 'trainingStart' | 'trainingEnd' | 'freePlaces' | 'maxPeople' | 'room' | 'isActive'
>;
@Component({
  selector: 'app-my-trainings-cms',
  imports: [ButtonLoaderComponent],
  templateUrl: './my-trainings-cms.component.html',
  styleUrl: './my-trainings-cms.component.scss'
})
export class MyTrainingsCMSComponent {

  trainings: TrainingModel[] = [];
  trainerApplicants: {
    accountId: number;
    id: number;
    acceptedAt: Date | null;
  }[] = [];
  displayTrainings: TrainingModel[] = [];
  originalTraining: TrainingModel | undefined;
  selectedTraining: Partial< TrainingModel> | undefined;
  searchTerm: string = '';
  deletingTrainingId: number | null = null;
  loadingTrainingId: number | null = null;
  bottomError: string | null = null;
  errorMsg: string = '';
  
  sortState: { property: SortableProperty | null; direction: SortDirection } = {
    property: null,
    direction: SortDirection.Asc,
  };
  
  constructor(
    private trainingService: TrainingService,
  ) {
    this.loadTrainings();
  }
  displayDate(training: TrainingModel) {
    const {trainingStart,trainingEnd } = training;
    console.log(trainingStart,trainingEnd)
    if(trainingStart)
    {
      const startString = `${trainingStart.getFullYear()}.${trainingStart.getDate()}.${trainingStart.getDay()} ${trainingStart.getHours()}:${trainingStart.getHours()} - `;
      if (trainingStart.getDay() == trainingEnd.getDay()) {
        return startString + `${trainingEnd.getHours()}:${trainingEnd.getMinutes()}`;
      }
      return startString + `${trainingEnd.getFullYear()}.${trainingEnd.getDate()}.${trainingEnd.getDay()} ${trainingEnd.getHours()}:${trainingEnd.getHours()}`;

    }
    else if(trainingEnd)
    {

      return`${trainingEnd.getFullYear()}.${trainingEnd.getDate()}.${trainingEnd.getDay()} ${trainingEnd.getHours()}:${trainingEnd.getHours()}`;
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
    
    let filtered = this.trainings.filter(s=> s.trainer?.toLocaleLowerCase().includes(this.searchTerm) || s.name?.toLocaleLowerCase().includes(this.searchTerm) || s.training?.toLocaleLowerCase().includes(this.searchTerm) || s.maxPeople?.toString().includes(this.searchTerm) || s.freePlaces?.toString().includes(this.searchTerm) ) ;
    
    console.log(this.sortState.property)
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
    if(this.deletingTrainingId) return;
    this.deletingTrainingId = training.id;
    this.trainingService.deleteTraining(training).subscribe({
      next: () => {
        this.trainings = this.trainings.filter(t=> t.id != this.deletingTrainingId)
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
          trainerId: 0,
          trainer: '',
          trainingEnd: new Date(),
          trainingStart: new Date(),
        };
      }
    }
}
