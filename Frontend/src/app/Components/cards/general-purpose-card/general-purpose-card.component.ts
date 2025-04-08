import { NgClass } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-general-purpose-card',
  imports: [NgClass,RouterLink],
  templateUrl: './general-purpose-card.component.html',
  styleUrl: './general-purpose-card.component.scss'
})
export class GeneralPurposeCardComponent {
cardClicked() {
  this.cardClickedEvent.emit(this);
}
  @Input() title?: string;
  @Input() benefits?: string[];
  @Input() price?: string;
  @Input() unit?: string;
  @Input() btnText?: string;
  @Input() linkBtnText?: string;
  @Input() linkTo?: string;
  @Input() imgSrc?: string;
  @Input() description?: string;
  @Input() isClickable?: boolean;
  @Input() viewTransition?: string;
  @Output() cardClickedEvent = new EventEmitter<typeof this>;
   
}
