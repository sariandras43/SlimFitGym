import { Component, input } from '@angular/core';
import { CardComponent } from "../card/card.component";

@Component({
  selector: 'app-machines',
  imports: [CardComponent],
  templateUrl: './machines.component.html',
  styleUrl: './machines.component.scss'
})
export class MachinesComponent {
  machines = input<Array<any>>();


}
