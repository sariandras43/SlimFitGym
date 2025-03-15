import { NgClass } from '@angular/common';
import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-general-purpose-card',
  imports: [NgClass,RouterLink],
  templateUrl: './general-purpose-card.component.html',
  styleUrl: './general-purpose-card.component.scss'
})
export class GeneralPurposeCardComponent {
title = input<string>();
   benefits = input<string[]>();
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
