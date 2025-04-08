import { Component, Input } from '@angular/core';
import { UserModel } from '../../Models/user.model';

@Component({
  selector: 'app-side-scroller',
  imports: [],
  templateUrl: './side-scroller.component.html',
  styleUrl: './side-scroller.component.scss'
})
export class SideScrollerComponent {
  
  
  @Input() trainers: {name?: string , imageUrl?: string}[] = [];
  get duplicatedUsers():  {name: string, imageUrl: string}[] {
    if (!this.trainers?.length) return [];
    
    if (!this.trainers?.length) return [];
  
    // Ensure minimum of 8 items for smooth animation
    const minItems = 8;
    const neededRepetitions = Math.ceil(minItems / this.trainers.length);
    return Array(neededRepetitions * 2).fill(this.trainers).flat();
  }
}
