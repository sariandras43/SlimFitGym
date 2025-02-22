import { NgClass } from '@angular/common';
import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-card',
  imports: [NgClass,RouterLink],
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
   imgSrc = input<string>()
   description = input<string>()
   isClickable = input<boolean>()
   viewTransition= input<string>()

  
}
