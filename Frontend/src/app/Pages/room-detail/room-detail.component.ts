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
import { UserService } from '../../Services/user.service';
import { UserModel } from '../../Models/user.model';
import { TrainingService } from '../../Services/training.service';
import { FooterComponent } from "../../Components/footer/footer.component";

@Component({
  selector: 'app-room-detail',
  imports: [
    HeroComponent,
    CounterBubbleComponent,
    EventsComponent,
    MachinesComponent,
    FooterComponent
],
  templateUrl: './room-detail.component.html',
  styleUrl: './room-detail.component.scss',
})
export class RoomDetailComponent {
unsubscribe(training: TrainingModel) {
  this.trainingService.unsubscribeFromTraining(training.id).subscribe({
    next: ()=>{
      training.freePlaces++;
      training.userApplied = false;
    },
    error: (err)=>{}
  })
}
subscribe(training: TrainingModel) {
  this.trainingService.subscribeToTraining(training.id).subscribe({
    next: ()=>{
      training.freePlaces--;
      training.userApplied = true;

    },
    error: (err)=>{}
  })
}
  id: number = 0;
  room: RoomModel | undefined;
  trainings: TrainingModel[] = [];
  allMachineCount: number = 0;
  machineTypeCount: number = 0;
  user: UserModel | undefined;
  constructor(private route: ActivatedRoute, roomService: RoomService, userService: UserService, private trainingService: TrainingService) {
    this.route.paramMap.subscribe((params) => {
      this.id = Number(params.get('id'));
    });
    userService.loggedInUser$.subscribe(usr=> this.user = usr);

    roomService.getRoom(this.id).subscribe({
      next: (response: RoomModel) => {
        this.room = response;
        response.machines.forEach((m) => {
          this.allMachineCount += m.machineCount || 0;
          this.machineTypeCount++;
        });
      },
      error: (error) => {
        console.log(error.error.message ?? error.message);
      },
    });

    
  }
  getTraining(){
    this.trainingService.getTrainingsInRoom(this.id).subscribe({
      next: (response: TrainingModel[]) => {
        this.trainings = response;

      },
      error: (error) => {
        console.log(error.error.message ?? error.message);
      },
    });
  }
  ngAfterViewInit() {
    this.getTraining();

  }

}
