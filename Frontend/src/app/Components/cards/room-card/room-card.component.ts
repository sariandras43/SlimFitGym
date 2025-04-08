import { Component, Input } from '@angular/core';
import { GeneralPurposeCardComponent } from "../general-purpose-card/general-purpose-card.component";
import { RoomModel } from '../../../Models/room.model';

@Component({
  selector: 'app-room-card',
  imports: [GeneralPurposeCardComponent],
  templateUrl: './room-card.component.html',
  styleUrl: './room-card.component.scss'
})
export class RoomCardComponent {
  @Input() room: RoomModel|undefined;
}
