import { Component } from '@angular/core';
import { GeneralPurposeCardComponent } from "../../cards/general-purpose-card/general-purpose-card.component";
import { MachineModel } from '../../../Models/machine.model';
import { MachineService } from '../../../Services/machine.service';

@Component({
  selector: 'app-machines-cms',
  imports: [GeneralPurposeCardComponent],
  templateUrl: './machines-cms.component.html',
  styleUrl: './machines-cms.component.scss'
})
export class MachinesCMSComponent {
machines:MachineModel[] | undefined;
constructor(machineService: MachineService) {
    machineService.allMachines$.subscribe((res)=>{ this.machines = res});
  }
}
