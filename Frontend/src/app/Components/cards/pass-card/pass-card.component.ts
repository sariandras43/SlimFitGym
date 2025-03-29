import { Component, EventEmitter, Input, Output, OutputEmitterRef, SimpleChanges } from '@angular/core';
import { PassModel } from '../../../Models/pass.model';
import { GeneralPurposeCardComponent } from "../general-purpose-card/general-purpose-card.component";
import { NgClass } from '@angular/common';
import { PassService } from '../../../Services/pass.service';
import { ButtonLoaderComponent } from "../../button-loader/button-loader.component";

@Component({
  selector: 'app-pass-card',
  imports: [GeneralPurposeCardComponent, NgClass, ButtonLoaderComponent],
  templateUrl: './pass-card.component.html',
  styleUrl: './pass-card.component.scss'
})
export class PassCardComponent {
buyPass(pass:PassModel | undefined) {
  if(!pass || pass.isLoading) return;
  this.boughtPass.emit(pass);

}
  @Input() pass: PassModel | undefined;
  @Input() isRoleUser: boolean = false;
  @Output() boughtPass = new EventEmitter<PassModel>(); 
   
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
