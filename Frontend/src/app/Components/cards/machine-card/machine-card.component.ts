import { Component, Input } from '@angular/core';
import { MachineModel } from '../../../Models/machine.model';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-machine-card',
  imports: [ NgClass],
  templateUrl: './machine-card.component.html',
  styleUrl: './machine-card.component.scss'
})
export class MachineCardComponent {
flipCard() {
  if(!this.machine?.imageUrls[1]) return
  this.flipped = !this.flipped;
}
  @Input() machine: MachineModel|undefined;
  flipped = false;
  showFullDescription = false;
  isDescriptionOverflow = false;

  ngAfterViewInit() {
    this.checkDescriptionOverflow();
  }

  toggleDescription(event: Event) {
    event.stopPropagation();
    this.showFullDescription = !this.showFullDescription;
  }

  private checkDescriptionOverflow() {
    const descElement = document.querySelector('.description');
    if (descElement) {
      this.isDescriptionOverflow = descElement.scrollHeight > descElement.clientHeight;
    }
  }
}
