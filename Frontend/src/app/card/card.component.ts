import { NgClass } from '@angular/common';
import { Component, input, output } from '@angular/core';
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
   expandable = input<boolean>(false);
   expanded = false;




   cardClicked() {
    //expands component if it can
    this.expanded = this.expandable() && !this.expanded;
  }
}
