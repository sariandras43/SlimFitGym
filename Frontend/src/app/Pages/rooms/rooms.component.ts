import { Component } from '@angular/core';
import { HeroComponent } from "../../Components/hero/hero.component";
import { CardComponent } from "../../Components/card/card.component";
import { CounterBubbleComponent } from "../../Components/counter-bubble/counter-bubble.component";

@Component({
  selector: 'app-rooms',
  imports: [HeroComponent, CardComponent, CounterBubbleComponent],
  templateUrl: './rooms.component.html',
  styleUrl: './rooms.component.scss'
})
export class RoomsComponent {

}
