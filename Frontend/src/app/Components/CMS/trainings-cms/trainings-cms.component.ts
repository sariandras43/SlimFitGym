import { Component } from '@angular/core';
import { TrainingModel } from '../../../Models/training.model';
import { TrainingService } from '../../../Services/training.service';
import { ButtonLoaderComponent } from "../../button-loader/button-loader.component";
import Utils from '../../../utils/util';

enum SortDirection {
  Asc = 'asc',
  Desc = 'desc',
}

type SortableProperty = keyof Pick<
  TrainingModel,
  'trainer' | 'name' | 'trainingStart' | 'trainingEnd' | 'freePlaces' | 'maxPeople' | 'room' | 'isActive'
>;


@Component({
  selector: 'app-trainings-cms',
  imports: [ButtonLoaderComponent],
  templateUrl: './trainings-cms.component.html',
  styleUrl: './trainings-cms.component.scss'
})
export class TrainingsCMSComponent {
toggleShowDeleted() {
  this.showDeleted = !this.showDeleted;
    this.updateDisplayTrainings();
}
  trainings: TrainingModel[] = [];
  trainerApplicants: {
    accountId: number;
    id: number;
    acceptedAt: Date | null;
  }[] = [];
  displayTrainings: TrainingModel[] = [];
  searchTerm: string = '';
  deletingTrainingId: number | null = null;
  loadingTrainingId: number | null = null;
  bottomError: string | null = null;
  errorMsg: string = '';
  showDeleted: boolean = false;
  
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
    return Utils.displayDate(training);  
  }
 
  private loadTrainings() {
    this.trainingService.getAllTrainings().subscribe({
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
    
    let filtered = this.trainings.filter(s=> s.trainer?.toLocaleLowerCase().includes(this.searchTerm) || s.name?.toLocaleLowerCase().includes(this.searchTerm) || s.room?.toLocaleLowerCase().includes(this.searchTerm) || s.maxPeople?.toString().includes(this.searchTerm) || s.freePlaces?.toString().includes(this.searchTerm) ) ;
    filtered = filtered.filter(t => this.showDeleted || t.isActive == true)
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
}
