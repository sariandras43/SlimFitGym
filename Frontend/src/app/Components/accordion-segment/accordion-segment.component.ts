import { Component, input } from '@angular/core';

@Component({
  selector: 'app-accordion-segment',
  imports: [],
  templateUrl: './accordion-segment.component.html',
  styleUrl: './accordion-segment.component.scss'
})
export class AccordionSegmentComponent {

title = input<string>();
opened : boolean = false
}
