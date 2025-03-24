import { Component, OnDestroy } from '@angular/core';
import { GeneralPurposeCardComponent } from '../../cards/general-purpose-card/general-purpose-card.component';
import { MachineModel } from '../../../Models/machine.model';
import { MachineService } from '../../../Services/machine.service';
import { Subject, takeUntil } from 'rxjs';
import { FormsModule } from '@angular/forms';

enum SortDirection {
  Asc = 'asc',
  Desc = 'desc',
}

type SortableProperty = keyof Pick<MachineModel, 'name' | 'description'>;

@Component({
  selector: 'app-machines-cms',
  imports: [FormsModule],
  templateUrl: './machines-cms.component.html',
  styleUrl: './machines-cms.component.scss',
})
export class MachinesCMSComponent {
  save() {
    if (this.selectedMachine) {
      this.machineService
        .saveMachine(this.selectedMachine)
        .subscribe({ next: () => {}, error: () => {} });
    }
  }
  machines: MachineModel[] = [];
  selectedMachine: MachineModel | undefined;
  sortState: { property: SortableProperty | null; direction: SortDirection } = {
    property: null,
    direction: SortDirection.Asc,
  };
  modalOpen(machine?: MachineModel) {
    if (machine) {
      this.selectedMachine = {
        description: machine.description,
        imageUrls: [...machine.imageUrls],
        name: machine.name,
        id: machine.id,
      };
    } else {
      this.selectedMachine = {
        description: '',
        imageUrls: [],
        name: '',
        id: -1,
      };
    }
  }

  imageChanged(event: Event, nth: number) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = (e: any) => {
        if (!this.selectedMachine) {
          return;
        }
        this.selectedMachine.imageUrls[nth] = e.target.result;
        const res = reader.result?.toString();
        if (res) {
          this.selectedMachine.imageUrls[nth] = res;
        }
      };
    }
  }

  constructor(private machineService: MachineService) {
    this.machineService.allMachines$.subscribe({
      next: (machines) => {
        if (machines) {
          this.machines = [...machines];
        }
      },
      error: (err) => console.error('Failed to load machines:', err),
    });
  }

  sortBy(property: SortableProperty): void {
    if (this.sortState.property === property) {
      this.sortState.direction =
        this.sortState.direction === SortDirection.Asc
          ? SortDirection.Desc
          : SortDirection.Asc;
    } else {
      this.sortState = {
        property: property,
        direction: SortDirection.Asc,
      };
    }

    this.machines = [...this.machines].sort((a, b) => {
      const valueA = String(a[property] ?? '').toLowerCase();
      const valueB = String(b[property] ?? '').toLowerCase();

      return this.sortState.direction === SortDirection.Asc
        ? valueA.localeCompare(valueB)
        : valueB.localeCompare(valueA);
    });
  }
  getSortIndicator(property: SortableProperty): string {
    if (this.sortState.property !== property) return '';
    return this.sortState.direction === SortDirection.Asc ? '⬆' : '⬇ ';
  }
}
