import { Component, ElementRef, Input, AfterViewInit, OnDestroy } from '@angular/core';
import { interval, take } from 'rxjs';

@Component({
  selector: 'app-counter-bubble',
  templateUrl: './counter-bubble.component.html',
  styleUrls: ['./counter-bubble.component.scss']
})
export class CounterBubbleComponent implements AfterViewInit, OnDestroy {
  @Input() number: number = 0;
  @Input() title: string = '';

  animatedNumber: number = 0;
  private observer!: IntersectionObserver;

  constructor(private elementRef: ElementRef) {}

  ngAfterViewInit() {
    this.observer = new IntersectionObserver(entries => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          this.startAnimation();
          this.observer.disconnect(); // Stop observing after first animation
        }
      });
    }, { threshold: 0.5 }); // Start animation when at least 50% visible

    this.observer.observe(this.elementRef.nativeElement);
  }

  startAnimation() {
    this.animatedNumber = 0; // Always start from 0

    if (this.number === 0) return; // No need to animate if target is 0

    const duration = 1000; // Animation duration in ms
    const steps = Math.min(this.number, 60); // Limit steps for smoothness
    const intervalTime = duration / steps;

    interval(intervalTime)
      .pipe(take(steps))
      .subscribe({
        next: step => this.animatedNumber = Math.round((step + 1) * (this.number / steps)),
        complete: () => this.animatedNumber = this.number
      });
  }

  ngOnDestroy() {
    if (this.observer) {
      this.observer.disconnect(); // Clean up observer when component is destroyed
    }
  }
}
