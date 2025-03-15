import { Component } from '@angular/core';
import { HeroComponent } from '../../Components/hero/hero.component';
import { CounterBubbleComponent } from '../../Components/counter-bubble/counter-bubble.component';
import { RoomService } from '../../Services/room.service';
import { RoomModel } from '../../Models/room.model';
import { RoomCardComponent } from "../../Components/cards/room-card/room-card.component";

@Component({
  selector: 'app-rooms',
  imports: [HeroComponent, CounterBubbleComponent, RoomCardComponent],
  templateUrl: './rooms.component.html',
  styleUrl: './rooms.component.scss',
})
export class RoomsComponent {
  rooms: RoomModel[] | undefined;
  constructor(roomService: RoomService) {
    roomService.rooms$.subscribe((rooms)=>{
      this.rooms = rooms;
    })
  }
}
