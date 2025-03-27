// room-cms.component.ts
import { Component } from '@angular/core';
import { RoomModel } from '../../../Models/room.model';
import { RoomService } from '../../../Services/room.service';
import { FormsModule } from '@angular/forms';
import { ButtonLoaderComponent } from '../../button-loader/button-loader.component';
import { MachineModel } from '../../../Models/machine.model';

enum SortDirection {
  Asc = 'asc',
  Desc = 'desc',
}

type SortableProperty = keyof Pick<RoomModel, 'name' | 'recommendedPeople'>;

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
    searchTerm: string = '';
    formSubmitted = false;
    nameError = false;
    priceError = false;
    isSubmitting = false;
    showDeleted = false;
    deletingRoomId: number | null = null;
    bottomError: string | null = null;
  
    sortState: { property: SortableProperty | null; direction: SortDirection } = {
      property: null,
      direction: SortDirection.Asc,
    };
  
    constructor(private roomService: RoomService) {
      this.loadRooms();
    }
  
    private loadRooms() {
      this.roomService.getRoomsAll().subscribe({
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
            room.machines.some((b) => b.name.toLowerCase().includes(this.searchTerm)))
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
        error: (err) => {console.error('Delete failed:', err)
  
          this.deletingRoomId = null;
        },
      });
    }
  
    save() {
      if (this.isSubmitting || !this.selectedRoom) return;
  
      this.formSubmitted = true;
     
  
      this.isSubmitting = true;
  
  
      this.roomService.saveRoom(this.selectedRoom).subscribe({
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
          this.bottomError = err.error?.message || 'Hiba történt a mentés során';
        },
      });
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
      this.selectedRoom = room
        ? { ...room, machines: [...room.machines] }
        : {
            id: -1,
            name: '',
            isActive: true,
            description: '',
            imageUrl: '',
            machines: [],
            recommendedPeople: 0
          };
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
