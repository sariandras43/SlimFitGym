import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-button-loader',
  imports: [],
  templateUrl: './button-loader.component.html',
  styleUrl: './button-loader.component.scss'
})
export class ButtonLoaderComponent {
  @Input() size: number = 1;
}
