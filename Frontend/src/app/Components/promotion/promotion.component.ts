import { Component, Input, ElementRef, OnInit } from '@angular/core';
import {
  trigger,
  state,
  style,
  transition,
  animate,
} from '@angular/animations';

@Component({
  selector: 'app-promotion',
  templateUrl: './promotion.component.html',
  styleUrls: ['./promotion.component.scss'],
  animations: [
    trigger('fadeInImage', [
      state(
        'hidden',
        style({
          opacity: 0,
          transform: 'translateX({{imageStart}})',
        }),
        { params: { imageStart: '100px' } }
      ),
      state(
        'visible',
        style({
          opacity: 1,
          transform: 'translateX(0)',
        })
      ),
      transition(
        'hidden => visible',
        animate('0.8s cubic-bezier(0.4, 0, 0.2, 1)')
      ),
    ]),
    trigger('fadeInText', [
      state(
        'hidden',
        style({
          opacity: 0,
          transform: 'translateX({{textStart}})',
        }),
        { params: { textStart: '-100px' } }
      ),
      state(
        'visible',
        style({
          opacity: 1,
          transform: 'translateX(0)',
        })
      ),
      transition(
        'hidden => visible',
        animate('0.8s cubic-bezier(0.4, 0, 0.2, 1)')
      ),
    ]),
  ],
})
export class PromotionComponent implements OnInit {
  @Input() align: 'left' | 'right' = 'left';
  @Input() imageUrl: string = '';
  @Input() title: string = '';
  @Input() description: string = '';
  animationState = 'hidden';
  private hasAnimated = false;

  constructor(private el: ElementRef) {}

  ngOnInit() {
    const observer = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting && !this.hasAnimated) {
            this.animationState = 'visible';
            this.hasAnimated = true;
          }
        });
      },
      {
        threshold: 0.1,
        rootMargin: '0px 0px -100px 0px',
      }
    );

    observer.observe(this.el.nativeElement);
  }
}
