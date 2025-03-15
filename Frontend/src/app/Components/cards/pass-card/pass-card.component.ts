import { Component, Input } from '@angular/core';
import { PassModel } from '../../../Models/pass.model';
import { GeneralPurposeCardComponent } from "../general-purpose-card/general-purpose-card.component";

@Component({
  selector: 'app-pass-card',
  imports: [GeneralPurposeCardComponent],
  templateUrl: './pass-card.component.html',
  styleUrl: './pass-card.component.scss'
})
export class PassCardComponent {
  @Input() pass: PassModel | undefined;

  getBenefits(pass: PassModel|undefined): string[] {
    if(!pass)
    {
      return [];
    }
    const benefits: string[] = [...pass.benefits];

    if (pass.days && pass.days > 0) {
      benefits.push(`${pass.days} napig érvényes`)

    }
    if (pass.maxEntries && pass.maxEntries > 0) {
      benefits.push(`${pass.maxEntries} belépés`)

    }
    else{
      benefits.push(`Korlátlan belépés`)

    }

    return benefits;
  }
}
