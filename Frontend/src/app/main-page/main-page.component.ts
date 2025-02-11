import { Component } from '@angular/core';
import { HeroComponent } from "../hero/hero.component";
import { CounterBubbleComponent } from "../counter-bubble/counter-bubble.component";
import { SubscriptionsComponent } from "../subscriptions/subscriptions.component";

@Component({
  selector: 'app-main-page',
  imports: [HeroComponent, CounterBubbleComponent, SubscriptionsComponent],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.scss'
})
export class MainPageComponent {

}
