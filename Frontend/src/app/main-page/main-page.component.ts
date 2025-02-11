import { Component } from '@angular/core';
import { HeroComponent } from "../hero/hero.component";
import { CounterBubbleComponent } from "../counter-bubble/counter-bubble.component";

@Component({
  selector: 'app-main-page',
  imports: [HeroComponent, CounterBubbleComponent],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.scss'
})
export class MainPageComponent {

}
