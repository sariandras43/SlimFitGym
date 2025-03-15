import { Component } from '@angular/core';
import { GeneralPurposeCardComponent } from "../general-purpose-card/general-purpose-card.component";

@Component({
  selector: 'app-machine-card',
  imports: [GeneralPurposeCardComponent],
  templateUrl: './machine-card.component.html',
  styleUrl: './machine-card.component.scss'
})
export class MachineCardComponent {

}
