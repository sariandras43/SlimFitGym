import { Component } from '@angular/core';
import { SubscriptionsComponent } from "../../Components/subscriptions/subscriptions.component";
import { HeroComponent } from "../../Components/hero/hero.component";
import { FooterComponent } from "../../Components/footer/footer.component";

@Component({
  selector: 'app-subscriptions-page',
  imports: [SubscriptionsComponent, FooterComponent],
  templateUrl: './subscriptions-page.component.html',
  styleUrl: './subscriptions-page.component.scss'
})
export class SubscriptionsPageComponent {

}
