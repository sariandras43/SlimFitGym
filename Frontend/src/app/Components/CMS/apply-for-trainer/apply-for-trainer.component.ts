import { Component } from '@angular/core';
import { TrainerApplicationService } from '../../../Services/trainer-application.service';
import { Observable } from 'rxjs';
import { ButtonLoaderComponent } from "../../button-loader/button-loader.component";

@Component({
  selector: 'app-apply-for-trainer',
  imports: [ButtonLoaderComponent],
  templateUrl: './apply-for-trainer.component.html',
  styleUrl: './apply-for-trainer.component.scss',
})
export class ApplyForTrainerComponent {
  errorMsg = '';
  loading = false;
  sent = false;
  applyToBeTrainer() {
    if (this.loading) return;
    this.loading = true;
    this.trainerApplicationService.applyForTrainer().subscribe({
      next: () => {
        this.loading = false;
        this.sent = true;
      },
      error: (err) => {
        this.loading = false;
        this.errorMsg =
        err.error?.message || 'Nem sikerült elutasítani a jelentkezést.';
       
      },
    });
  }
  constructor(private trainerApplicationService: TrainerApplicationService) {}
}
