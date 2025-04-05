// room-cms.component.ts
import { Component } from '@angular/core';
import { RoomModel } from '../../../Models/room.model';
import { RoomService } from '../../../Services/room.service';
import { FormsModule } from '@angular/forms';
import { ButtonLoaderComponent } from '../../button-loader/button-loader.component';
import { MachineModel } from '../../../Models/machine.model';
import { MachinesCMSComponent } from '../machines-cms/machines-cms.component';
import { MachineService } from '../../../Services/machine.service';

enum SortDirection {
  Asc = 'asc',
  Desc = 'desc',
}

type SortableProperty = keyof Pick<
  RoomModel,
  'name' | 'description' | 'recommendedPeople' | 'isActive'
>;

@Component({
  selector: 'app-room-cms',
  standalone: true,
  imports: [FormsModule, ButtonLoaderComponent],
  templateUrl: './room-cms.component.html',
  styleUrls: ['./room-cms.component.scss'],
})
export class RoomCMSComponent {
  rooms: RoomModel[] = [];
  displayRooms: RoomModel[] = [];
  selectedRoom: RoomModel | undefined;
  originalRoom: RoomModel | undefined;
  searchTerm: string = '';
  formSubmitted = false;
  nameError = false;
  recomendedPeopleError = false;
  descriptionError = false;
  isSubmitting = false;
  showDeleted = false;
  deletingRoomId: number | null = null;
  bottomError: string | null = null;
  machines: MachineModel[] = [];
  selectedMachineId: number | null = null;

  sortState: { property: SortableProperty | null; direction: SortDirection } = {
    property: null,
    direction: SortDirection.Asc,
  };
  addSelectedMachine() {
    if (!this.selectedMachineId || !this.selectedRoom) return;
    const machine = this.machines.find((m) => m.id == this.selectedMachineId);
    console.log(this.selectedMachineId);
    if (machine) {
      this.selectedRoom.machines = [
        ...this.selectedRoom.machines,
        { ...machine, machineCount: 1 },
      ];
      this.selectedMachineId = null;
    }
  }
  constructor(
    private roomService: RoomService,
    private machineService: MachineService
  ) {
    this.loadRooms();
    this.loadMachines();
  }
  private loadMachines() {
    this.machineService.allMachines$.subscribe({
      next: (machines) => {
        this.machines = machines || [];
      },
      error: (err) => console.error('Failed to load rooms:', err),
    });
  }

  imageChanged(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = (e: any) => {
        if (!this.selectedRoom) return;
        this.selectedRoom.imageUrl = e.target.result;
      };
    }
  }

  private loadRooms() {
    this.roomService.rooms$.subscribe({
      next: (rooms) => {
        this.rooms = rooms || [];
        this.updateDisplayRooms();
      },
      error: (err) => console.error('Failed to load rooms:', err),
    });
  }

  search(event: Event) {
    const input = event.target as HTMLInputElement;
    this.searchTerm = input.value.toLowerCase();
    this.updateDisplayRooms();
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
    this.updateDisplayRooms();
  }

  private updateDisplayRooms() {
    let filtered = this.rooms.filter(
      (room) =>
        (room.isActive || this.showDeleted) &&
        (room.name.toLowerCase().includes(this.searchTerm) ||
          room.machines.some((b) =>
            b.name.toLowerCase().includes(this.searchTerm)
          ))
    );

    if (this.sortState.property) {
      filtered = this.sortrooms(
        filtered,
        this.sortState.property,
        this.sortState.direction
      );
    }

    this.displayRooms = filtered;
  }

  private sortrooms(
    rooms: RoomModel[],
    property: SortableProperty,
    direction: SortDirection
  ) {
    return [...rooms].sort((a, b) => {
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

  delete(room: RoomModel) {
    if (!room.isActive) return;
    this.deletingRoomId = room.id;
    this.roomService.deleteRoom(room).subscribe({
      next: () => {
        room.isActive = false;
        this.deletingRoomId = null;
        this.updateDisplayRooms();
      },
      error: (err) => {
        console.error('Delete failed:', err);

        this.deletingRoomId = null;
      },
    });
  }
  private arraysEqual(a: MachineModel[], b: MachineModel[]): boolean {
    if (a === b) return true;
    if (a == null || b == null) return false;
    if (a.length !== b.length) return false;

    for (let i = 0; i < a.length; ++i) {
      console.log(a[i].machineCount, b[i].machineCount);
      if (a[i].id !== b[i].id) return false;
      if (a[i].machineCount !== b[i].machineCount) return false;
    }
    return true;
  }

  save() {
    if (this.isSubmitting || !this.selectedRoom) return;

    this.formSubmitted = true;

    this.isSubmitting = true;

    if(this.selectedRoom.name == '')
    {
      this.nameError = true;
      return;
    }
    if(this.selectedRoom.description == '')
      {
        this.descriptionError = true;
        return;
      }
      if(this.selectedRoom.recommendedPeople == 0)
        {
          this.recomendedPeopleError = true;
          return;
        }

    if (this.selectedRoom.id === -1) {
      this.roomService.saveRoom(this.selectedRoom).subscribe({
        next: (updated) => {
          this.rooms.unshift(updated);
          this.selectedRoom = undefined;
          this.isSubmitting = false;
          this.updateDisplayRooms();
        },
        error: (err) => {
          console.error(err);
          this.isSubmitting = false;
          this.bottomError =
            err.error?.message || 'Hiba történt mentés közben.';
        },
      });
    } else {
      if (!this.originalRoom) {
        this.isSubmitting = false;
        this.bottomError = 'Nem található az eredeti szoba.';
        return;
      }

      const payload: Partial<RoomModel> = { id: this.originalRoom.id };

      if (this.selectedRoom.name !== this.originalRoom.name)
        payload.name = this.selectedRoom.name;
      if (this.selectedRoom.description !== this.originalRoom.description)
        payload.description = this.selectedRoom.description;
      if (this.selectedRoom.image !== this.originalRoom.image)
        payload.image = this.selectedRoom.image;
      if (
        this.selectedRoom.recommendedPeople !==
        this.originalRoom.recommendedPeople
      )
        payload.recommendedPeople = this.selectedRoom.recommendedPeople;

      if (this.selectedRoom?.imageUrl !== this.originalRoom?.imageUrl) {
        payload.image = this.selectedRoom.imageUrl;
      }
      const originalMachines = this.originalRoom.machines.sort();
      const selectedMachines = this.selectedRoom.machines.sort();
      if (!this.arraysEqual(originalMachines, selectedMachines)) {
        payload.machines = this.selectedRoom.machines;
      }

      if (Object.keys(payload).length === 1) {
        this.selectedRoom = undefined;
        this.isSubmitting = false;
        return;
      }

      this.roomService.saveRoom(payload as RoomModel).subscribe({
        next: (updated) => {
          const index = this.rooms.findIndex((p) => p.id === updated.id);
          if (index > -1) {
            this.rooms[index] = updated;
          } else {
            this.rooms.unshift(updated);
          }
          this.selectedRoom = undefined;
          this.isSubmitting = false;
          this.updateDisplayRooms();
        },
        error: (err) => {
          console.error(err);
          this.isSubmitting = false;
          this.bottomError = err.error?.message || 'Hiba mentés közben.';
        },
      });
    }
  }

  addMachine(machine: MachineModel) {
    if (this.selectedRoom) {
      this.selectedRoom.machines = [...this.selectedRoom.machines, machine];
    }
  }

  removeBenefit(index: number) {
    if (this.selectedRoom) {
      this.selectedRoom.machines = this.selectedRoom.machines.filter(
        (_, i) => i !== index
      );
    }
  }

  modalOpen(room?: RoomModel) {
    this.bottomError = null;
    if (room) {
      const original = this.rooms.find((r) => r.id === room.id);
      if (original) {
        this.originalRoom = {
          ...original,
          machines: original.machines.map((m) => ({ ...m })),
        };
        this.selectedRoom = {
          ...original,
          machines: original.machines.map((m) => ({ ...m })),
        };
      }
    } else {
      this.originalRoom = undefined;
      this.selectedRoom = {
        id: -1,
        name: '',
        description: '',
        image: '',
        machines: [],
        recommendedPeople: 0,
      };
    }
  }

  toggleShowDeleted() {
    this.showDeleted = !this.showDeleted;
    this.updateDisplayRooms();
  }

  getSortIndicator(property: SortableProperty): string {
    if (this.sortState.property !== property) return '';
    return this.sortState.direction === SortDirection.Asc ? '⬆' : '⬇';
  }
}
