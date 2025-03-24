import { Component, Input, input } from '@angular/core';
import { HeroComponent } from '../../Components/hero/hero.component';
import { CounterBubbleComponent } from '../../Components/counter-bubble/counter-bubble.component';
import { EventsComponent } from '../../Components/events/events.component';
import { MachinesComponent } from '../../Components/machines/machines.component';
import { MachineModel } from '../../Models/machine.model';
import { RoomService } from '../../Services/room.service';
import { RoomModel } from '../../Models/room.model';
import { ActivatedRoute } from '@angular/router';
import { TrainingModel } from '../../Models/training.model';

@Component({
  selector: 'app-room-detail',
  imports: [
    HeroComponent,
    CounterBubbleComponent,
    EventsComponent,
    MachinesComponent,
  ],
  templateUrl: './room-detail.component.html',
  styleUrl: './room-detail.component.scss',
})
export class RoomDetailComponent {
  id: number = 0;
  room: RoomModel | undefined;
  trainings: TrainingModel[] = [];
  allMachineCount: number = 0;
  machineTypeCount: number = 0;
  constructor(private route: ActivatedRoute, roomService: RoomService) {
    this.route.paramMap.subscribe((params) => {
      this.id = Number(params.get('id'));
    });
    roomService.getRoom(this.id).subscribe({
      next: (response: RoomModel) => {
        this.room = response;
        response.machines.forEach((m) => {
          this.allMachineCount += m.machineCount || 0;
          this.machineTypeCount++;
        });
        console.log(this.machineTypeCount)
      },
      error: (error) => {
        console.log(error.error.message ?? error.message);
      },
    });

    roomService.getTrainingsInRoom(this.id).subscribe({
      next: (response: TrainingModel[]) => {
        this.trainings = response;

      },
      error: (error) => {
        console.log(error.error.message ?? error.message);
      },
    });
  }


}
