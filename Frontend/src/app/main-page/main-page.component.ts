import { Component } from '@angular/core';
import { HeroComponent } from "../hero/hero.component";

@Component({
  selector: 'app-main-page',
  imports: [HeroComponent],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.scss'
})
export class MainPageComponent {

}
