import { Component } from '@angular/core';
import { HeroComponent } from '../../Components/hero/hero.component';
import { CounterBubbleComponent } from '../../Components/counter-bubble/counter-bubble.component';
import { RoomService } from '../../Services/room.service';
import { RoomModel } from '../../Models/room.model';
import { RoomCardComponent } from "../../Components/cards/room-card/room-card.component";
import { MachineService } from '../../Services/machine.service';
import { FooterComponent } from "../../Components/footer/footer.component";

@Component({
  selector: 'app-rooms',
  imports: [HeroComponent, CounterBubbleComponent, RoomCardComponent, FooterComponent],
  templateUrl: './rooms.component.html',
  styleUrl: './rooms.component.scss',
})
export class RoomsComponent {
  rooms: RoomModel[] | undefined;
  fullMaxPeople = 0;
  machineCount = 0;
  constructor(private roomService: RoomService, private machineService: MachineService) {
    
  }
  ngAfterViewInit() {
    this.roomService.rooms$.subscribe((rooms) => {
      this.rooms = rooms;
      this.fullMaxPeople = 0;
      rooms?.forEach(r=>{
        this.fullMaxPeople += r.recommendedPeople;
      })
      this.machineService.allMachines$.subscribe(m=>{
        this.machineCount = m?.length || 0;
       
      })
    });
  }
}
