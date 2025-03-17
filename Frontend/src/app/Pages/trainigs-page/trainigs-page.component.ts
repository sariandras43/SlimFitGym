import { Component } from '@angular/core';
import { HeroComponent } from "../../Components/hero/hero.component";
import { TrainingModel } from '../../Models/training.model';
import { TrainingService } from '../../Services/training.service';

@Component({
  selector: 'app-trainigs-page',
  imports: [HeroComponent],
  templateUrl: './trainigs-page.component.html',
  styleUrl: './trainigs-page.component.scss'
})
export class TrainigsPageComponent {
  trainings: TrainingModel[] | undefined;
  constructor(trainingservice: TrainingService) {
    trainingservice.allTrainings$.subscribe((trainings)=>{
      this.trainings = trainings;
    })
  }

}
