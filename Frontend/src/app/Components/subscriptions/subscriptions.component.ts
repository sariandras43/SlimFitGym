import { Component, input } from '@angular/core';
import { CardComponent } from '../card/card.component';
import { NgClass } from '@angular/common';
import { PassService } from '../../Services/pass.service';
import { PassModel } from '../../Models/pass.model';

@Component({
  selector: 'app-subscriptions',
  imports: [CardComponent, NgClass],
  templateUrl: './subscriptions.component.html',
  styleUrl: './subscriptions.component.scss',
})
export class SubscriptionsComponent {
  highlightOnly = input<boolean>();
  title = input<string>();
  passes: PassModel[] | undefined;
  constructor(passService: PassService) {
    passService.getPasses();
    passService.allPasses$.subscribe((passes) => {
      console.log(passes);
      this.passes = passes;
    });
  }

  getBenefits(pass: PassModel): string[] {
    const benefits: string[] = [...pass.benefits];

    if (pass.days && pass.days > 0) {
      benefits.push(`${pass.days} napig`)

    }
    if (pass.maxEntries && pass.maxEntries > 0) {
      benefits.push(`${pass.maxEntries} belépés`)

    }

    return benefits;
  }
}
