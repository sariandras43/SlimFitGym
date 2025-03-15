import { Component, input } from '@angular/core';
import { MachineCardComponent } from '../cards/machine-card/machine-card.component';
import { MachineService } from '../../Services/machine.service';
import { MachineModel } from '../../Models/machine.model';

@Component({
  selector: 'app-machines',
  imports: [MachineCardComponent],
  templateUrl: './machines.component.html',
  styleUrl: './machines.component.scss',
})
export class MachinesComponent {
  machines = input<MachineModel[]>();

  
  




}
