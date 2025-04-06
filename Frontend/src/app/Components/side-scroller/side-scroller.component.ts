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
    
    const minItems =  this.trainers.length * 1 > 8 ? this.trainers.length : 8; 
    const repeatCount = Math.ceil(minItems / this.trainers.length);
    return Array(repeatCount).fill(this.trainers).flat();
  }
}
