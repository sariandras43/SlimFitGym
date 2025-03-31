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
  SearchTerm: string = '';
search($event: Event) {
  const input = $event.target as HTMLInputElement;
  this.SearchTerm = input.value.toLowerCase();
  this.updateMachines();
}
machines: MachineModel[] | undefined;
displayMachines: MachineModel[] | undefined;
constructor(machineService: MachineService) {
  machineService.allMachines$.subscribe((res)=>{ this.machines = res});
  this.updateMachines();
}
updateMachines(){
   this.displayMachines = this.machines?.filter(s=> s.description?.toLocaleLowerCase().includes(this.SearchTerm) || s.name.toLocaleLowerCase().includes(this.SearchTerm))

  }
}
