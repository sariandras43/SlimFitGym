import { Component } from '@angular/core';
import { HeroComponent } from '../../Components/hero/hero.component';
import { MachinesComponent } from '../../Components/machines/machines.component';
import { MachineService } from '../../Services/machine.service';
import { MachineModel } from '../../Models/machine.model';

@Component({
  selector: 'app-machines-page',
  imports: [HeroComponent, MachinesComponent],
  templateUrl: './machines-page.component.html',
  styleUrl: './machines-page.component.scss',
})
export class MachinesPageComponent {
  machines: MachineModel[] | undefined;
  constructor(machineService: MachineService) {
    machineService.allMachines$.subscribe((res)=>{ this.machines = res});
  }
}
