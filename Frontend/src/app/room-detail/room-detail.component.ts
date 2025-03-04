import { Component } from '@angular/core';
import { HeroComponent } from "../hero/hero.component";
import { CounterBubbleComponent } from "../counter-bubble/counter-bubble.component";
import { CardComponent } from "../card/card.component";
import { EventsComponent } from "../events/events.component";
import { MachinesComponent } from "../machines/machines.component";

@Component({
  selector: 'app-room-detail',
  imports: [HeroComponent, CounterBubbleComponent, CardComponent, EventsComponent, MachinesComponent],
  templateUrl: './room-detail.component.html',
  styleUrl: './room-detail.component.scss'
})
export class RoomDetailComponent {
  machines = Array<string>(10)
}
