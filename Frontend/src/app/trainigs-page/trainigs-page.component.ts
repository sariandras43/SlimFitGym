import { Component } from '@angular/core';
import { HeroComponent } from "../hero/hero.component";

@Component({
  selector: 'app-trainigs-page',
  imports: [HeroComponent],
  templateUrl: './trainigs-page.component.html',
  styleUrl: './trainigs-page.component.scss'
})
export class TrainigsPageComponent {
  trainings = Array<string>(10)
}
