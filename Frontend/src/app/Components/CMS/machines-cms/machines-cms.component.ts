import { Component, OnDestroy } from '@angular/core';
import { GeneralPurposeCardComponent } from '../../cards/general-purpose-card/general-purpose-card.component';
import { MachineModel } from '../../../Models/machine.model';
import { MachineService } from '../../../Services/machine.service';
import { Subject, takeUntil } from 'rxjs';

enum SortDirection {
  Asc = 'asc',
  Desc = 'desc',
}

type SortableProperty = keyof Pick<MachineModel, 'name' | 'description'>;

@Component({
  selector: 'app-machines-cms',
  imports: [GeneralPurposeCardComponent],
  templateUrl: './machines-cms.component.html',
  styleUrl: './machines-cms.component.scss',
})
export class MachinesCMSComponent {
 
  machines: MachineModel[] = [];
  selectedMachine: | MachineModel = {description: '', id: 0, imageUrls: [], name:''};
  sortState: { property: SortableProperty | null; direction: SortDirection } = {
    property: null,
    direction: SortDirection.Asc,
  };

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

  imageChanged(event: Event, nth: number) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = (e: any) => {
        this.selectedMachine.imageUrls[nth] = e.target.result;
        const res = reader.result?.toString();
        if (res) {
          this.selectedMachine.imageUrls[nth] = res;
          
        }
      };
    }
  }
}
