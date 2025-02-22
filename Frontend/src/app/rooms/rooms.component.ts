import { Component } from '@angular/core';
import { HeroComponent } from "../hero/hero.component";
import { CardComponent } from "../card/card.component";
import { CounterBubbleComponent } from "../counter-bubble/counter-bubble.component";

@Component({
  selector: 'app-rooms',
  imports: [HeroComponent, CardComponent, CounterBubbleComponent],
  templateUrl: './rooms.component.html',
  styleUrl: './rooms.component.scss'
})
export class RoomsComponent {

}
