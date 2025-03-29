import { Component } from '@angular/core';
import { MachineModel } from '../../../Models/machine.model';
import { MachineService } from '../../../Services/machine.service';
import { FormsModule } from '@angular/forms';
import { ButtonLoaderComponent } from "../../button-loader/button-loader.component";

enum SortDirection {
  Asc = 'asc',
  Desc = 'desc',
}

type SortableProperty = keyof Pick<MachineModel, 'name' | 'description'>;

@Component({
  selector: 'app-machines-cms',
  imports: [FormsModule, ButtonLoaderComponent],
  templateUrl: './machines-cms.component.html',
  styleUrl: './machines-cms.component.scss',
})
export class MachinesCMSComponent {
  machines: MachineModel[] = [];
  displayMachines: MachineModel[] = [];
  selectedMachine: MachineModel | undefined;
  originalMachine: MachineModel | undefined;
  searchTerm: string = '';
  bottomError: string = '';
  formSubmitted = false;
  nameError = false;
  isSubmitting = false;
  deletingMachineId: number | null = null;
  sortState: { property: SortableProperty | null; direction: SortDirection } = {
    property: null,
    direction: SortDirection.Asc,
  };

  constructor(private machineService: MachineService) {
    this.machineService.allMachines$.subscribe({
      next: (machines) => {
        if (machines) {
          this.machines = [...machines];
          this.updateDisplayMachines();
        }
      },
      error: (err) => console.error('Failed to load machines:', err),
    });
  }

  search($event: Event) {
    const input = $event.target as HTMLInputElement;
    this.searchTerm = input.value.toLowerCase();
    this.updateDisplayMachines();
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
    this.updateDisplayMachines();
    
  }

  private updateDisplayMachines() {
    let filteredMachines = this.machines.filter(
      (machine) =>
        machine.name.toLowerCase().includes(this.searchTerm) ||
        machine.description?.toLowerCase().includes(this.searchTerm)
    );

    if (this.sortState.property) {
      filteredMachines = this.sortMachines(
        filteredMachines,
        this.sortState.property,
        this.sortState.direction
      );
    }

    this.displayMachines = filteredMachines;
  }

  private sortMachines(
    machines: MachineModel[],
    property: SortableProperty,
    direction: SortDirection
  ): MachineModel[] {
    return [...machines].sort((a, b) => {
      const valueA = String(a[property] ?? '').toLowerCase();
      const valueB = String(b[property] ?? '').toLowerCase();
      return direction === SortDirection.Asc
        ? valueA.localeCompare(valueB)
        : valueB.localeCompare(valueA);
    });
  }

  delete(machine: MachineModel) {
    if (this.deletingMachineId !== null) return;
    this.deletingMachineId = machine.id;
    this.machineService.deleteMachine(machine).subscribe({
      next: (deletedMachine) => {
        this.machines = this.machines.filter((m) => m.id != deletedMachine.id);
        this.displayMachines = this.displayMachines.filter((m) => m.id != deletedMachine.id);
        this.deletingMachineId = null;
      },
      error: (err) => {
        console.log(err);
        this.deletingMachineId = null;
      },
    });
  }

  save() {
    if (this.isSubmitting || !this.selectedMachine) return;
  
    this.formSubmitted = true;
    this.nameError = !this.selectedMachine.name.trim();
  
    const hasMain = !!this.selectedMachine.imageUrls[0];
    const hasSecondary = !!this.selectedMachine.imageUrls[1];
    const imagePairError = !hasMain && hasSecondary;
  
    if (this.nameError) return;
    if (imagePairError) {
      this.bottomError = 'Segédleti kép feltöltése csak fő képpel együtt lehetséges!';
      return;
    }
  
    this.isSubmitting = true;
  
    const payload: Partial<MachineModel> = { id: this.selectedMachine.id };
  
    if (this.selectedMachine.id === -1) {
      payload.name = this.selectedMachine.name;
      payload.description = this.selectedMachine.description;
      payload.imageUrls = this.selectedMachine.imageUrls;
    } else {
      if (!this.originalMachine) {
        this.isSubmitting = false;
        this.bottomError = 'Nem található az eredeti gép.';
        return;
      }
  
      if (this.selectedMachine.name !== this.originalMachine.name) {
        payload.name = this.selectedMachine.name;
      }
      if (this.selectedMachine.description !== this.originalMachine.description) {
        payload.description = this.selectedMachine.description;
      }
      if (!this.arraysEqual(this.selectedMachine.imageUrls, this.originalMachine.imageUrls)) {
        payload.imageUrls = this.selectedMachine.imageUrls;
      }
    }
  
    if (Object.keys(payload).length === 1 && this.selectedMachine.id !== -1) {
      this.selectedMachine = undefined;
      this.isSubmitting = false;
      return;
    }
  
    this.machineService.saveMachine(payload as MachineModel).subscribe({
      next: (updatedMachine) => {
        const index = this.machines.findIndex(m => m.id === updatedMachine.id);
        if (index > -1) {
          this.machines[index] = updatedMachine;
        } else {
          this.machines.unshift(updatedMachine);
        }
        this.selectedMachine = undefined;
        this.isSubmitting = false;
        this.updateDisplayMachines();
      },
      error: (error) => {
        this.bottomError = error.error?.message || error.message;
        this.isSubmitting = false;
      },
    });
  }
  
  private arraysEqual(a: string[], b: string[]): boolean {
    if (a === b) return true;
    if (a.length !== b.length) return false;
    return a.every((val, index) => val === b[index]);
  }

  modalOpen(machine?: MachineModel) {
    this.bottomError = '';
    if (machine) {
      const original = this.machines.find(m => m.id === machine.id);
      if (original) {
        this.originalMachine = { 
          ...original, 
          imageUrls: [...original.imageUrls] 
        };
        this.selectedMachine = { 
          ...original, 
          imageUrls: [...original.imageUrls] 
        };
      }
    } else {
      this.originalMachine = undefined;
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

  getSortIndicator(property: SortableProperty): string {
    if (this.sortState.property !== property) return '';
    return this.sortState.direction === SortDirection.Asc ? '⬆' : '⬇ ';
  }
}
