import { Component } from '@angular/core';
import { SubscriptionsComponent } from "../subscriptions/subscriptions.component";
import { HeroComponent } from "../hero/hero.component";

@Component({
  selector: 'app-subscriptions-page',
  imports: [SubscriptionsComponent, HeroComponent],
  templateUrl: './subscriptions-page.component.html',
  styleUrl: './subscriptions-page.component.scss'
})
export class SubscriptionsPageComponent {

}
