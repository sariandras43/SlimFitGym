import { Component, input } from '@angular/core';
import { CardComponent } from "../card/card.component";
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-subscriptions',
  imports: [CardComponent, NgClass],
  templateUrl: './subscriptions.component.html',
  styleUrl: './subscriptions.component.scss'
})
export class SubscriptionsComponent {
  highlightOnly = input<boolean>()

}
