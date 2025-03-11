import { Component } from '@angular/core';
import { HeroComponent } from "../hero/hero.component";
import { MachinesComponent } from "../machines/machines.component";

@Component({
  selector: 'app-machines-page',
  imports: [HeroComponent, MachinesComponent],
  templateUrl: './machines-page.component.html',
  styleUrl: './machines-page.component.scss'
})
export class MachinesPageComponent {
  machines = Array<string>(10)
}
