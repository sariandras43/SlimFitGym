import { Component } from '@angular/core';
import { SubscriptionsComponent } from "../../Components/subscriptions/subscriptions.component";
import { HeroComponent } from "../../Components/hero/hero.component";

@Component({
  selector: 'app-subscriptions-page',
  imports: [SubscriptionsComponent],
  templateUrl: './subscriptions-page.component.html',
  styleUrl: './subscriptions-page.component.scss'
})
export class SubscriptionsPageComponent {

}
