import { Component } from '@angular/core';
import { HeroComponent } from "../../Components/hero/hero.component";
import { CounterBubbleComponent } from "../../Components/counter-bubble/counter-bubble.component";
import { EventsComponent } from "../../Components/events/events.component";
import { MachinesComponent } from "../../Components/machines/machines.component";
import { MachineModel } from '../../Models/machine.model';

@Component({
  selector: 'app-room-detail',
  imports: [HeroComponent, CounterBubbleComponent, EventsComponent, MachinesComponent],
  templateUrl: './room-detail.component.html',
  styleUrl: './room-detail.component.scss'
})
export class RoomDetailComponent {
  machines = Array<MachineModel>(10)
}
