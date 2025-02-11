import { Component, input } from '@angular/core';

@Component({
  selector: 'app-card',
  imports: [],
  templateUrl: './card.component.html',
  styleUrl: './card.component.scss'
})
export class CardComponent {
  benefits = input<string[]>();
   title = input<string>();
   price = input<string>();
   unit = input<string>();
   btnText = input<string>();
   linkBtnText = input<string>()
   linkTo = input<string>()

}
