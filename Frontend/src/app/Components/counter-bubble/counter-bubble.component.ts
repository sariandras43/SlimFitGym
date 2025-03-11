import { Component, input } from '@angular/core';

@Component({
  selector: 'app-counter-bubble',
  imports: [],
  templateUrl: './counter-bubble.component.html',
  styleUrl: './counter-bubble.component.scss'
})
export class CounterBubbleComponent {
  number = input<number>();
  title = input<string>();
}
