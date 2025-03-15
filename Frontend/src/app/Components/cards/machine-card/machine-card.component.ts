import { Component, Input } from '@angular/core';
import { GeneralPurposeCardComponent } from "../general-purpose-card/general-purpose-card.component";
import { MachineModel } from '../../../Models/machine.model';

@Component({
  selector: 'app-machine-card',
  imports: [GeneralPurposeCardComponent],
  templateUrl: './machine-card.component.html',
  styleUrl: './machine-card.component.scss'
})
export class MachineCardComponent {
  @Input() machine: MachineModel|undefined;
}
