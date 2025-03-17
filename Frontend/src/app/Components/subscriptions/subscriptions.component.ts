import { Component, input } from '@angular/core';
import { NgClass } from '@angular/common';
import { PassService } from '../../Services/pass.service';
import { PassModel } from '../../Models/pass.model';
import { PassCardComponent } from "../cards/pass-card/pass-card.component";

@Component({
  selector: 'app-subscriptions',
  imports: [NgClass, PassCardComponent],
  templateUrl: './subscriptions.component.html',
  styleUrl: './subscriptions.component.scss',
})
export class SubscriptionsComponent {
  highlightOnly = input<boolean>();
  title = input<string>();
  passes: PassModel[] | undefined;
  constructor(passService: PassService) {
    passService.allPasses$.subscribe((passes) => {
      this.passes = passes?.sort((a,b)=> a.price-b.price);
    });
  }

  
}
