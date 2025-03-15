import { Component } from '@angular/core';
import { HeroComponent } from "../../Components/hero/hero.component";
import { CounterBubbleComponent } from "../../Components/counter-bubble/counter-bubble.component";
import { SubscriptionsComponent } from "../../Components/subscriptions/subscriptions.component";
import { PromotionComponent } from "../../Components/promotion/promotion.component";

@Component({
  selector: 'app-main-page',
  imports: [HeroComponent, CounterBubbleComponent, SubscriptionsComponent, PromotionComponent],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.scss'
})
export class MainPageComponent {

}
