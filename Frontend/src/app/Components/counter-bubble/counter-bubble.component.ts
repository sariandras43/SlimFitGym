import { Component, ElementRef, Input, AfterViewInit, OnDestroy, SimpleChanges, OnChanges } from '@angular/core';
import { interval, take, Subscription } from 'rxjs';

@Component({
  selector: 'app-counter-bubble',
  templateUrl: './counter-bubble.component.html',
  styleUrls: ['./counter-bubble.component.scss']
})
export class CounterBubbleComponent implements AfterViewInit, OnDestroy, OnChanges {
  @Input() number: number = 0;
  @Input() title: string = '';

  animatedNumber: number = 0;
  private isVisible = false;
  private observer!: IntersectionObserver;
  private animationSubscription?: Subscription;

  constructor(private elementRef: ElementRef) {}

  ngAfterViewInit() {
    this.observer = new IntersectionObserver(entries => {
      entries.forEach(entry => {
        this.isVisible = entry.isIntersecting;
        if (this.isVisible && this.animatedNumber !== this.number) {
          this.startAnimation();
        }
      });
    }, { threshold: 0.5 });

    this.observer.observe(this.elementRef.nativeElement);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['number'] && !changes['number'].isFirstChange()) {
      if (this.isVisible) {
        this.startAnimation();
      }
    }
  }

  startAnimation() {
    if (this.animationSubscription) {
      this.animationSubscription.unsubscribe();
    }

    this.animatedNumber = 0;

    if (this.number === 0) return;

    const duration = 1000;
    const steps = Math.min(this.number, 60);
    const intervalTime = duration / steps;

    this.animationSubscription = interval(intervalTime)
      .pipe(take(steps))
      .subscribe({
        next: (step) => this.animatedNumber = Math.round((step + 1) * (this.number / steps)),
        complete: () => this.animatedNumber = this.number
      });
  }

  ngOnDestroy() {
    if (this.animationSubscription) {
      this.animationSubscription.unsubscribe();
    }
    if (this.observer) {
      this.observer.disconnect();
    }
  }
}