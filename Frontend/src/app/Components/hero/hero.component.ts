import { Component, input, Input } from '@angular/core';

@Component({
  selector: 'app-hero',
  imports: [],
  templateUrl: './hero.component.html',
  styleUrl: './hero.component.scss'
})
export class HeroComponent {
  imgSrc = input<string>();
  title = input<string>();
  subtitle = input<string>();

  
}
