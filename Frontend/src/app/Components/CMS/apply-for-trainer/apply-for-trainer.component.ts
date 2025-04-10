import { Component, Input } from '@angular/core';
import { TrainerApplicationService } from '../../../Services/trainer-application.service';
import { Observable } from 'rxjs';
import { ButtonLoaderComponent } from '../../button-loader/button-loader.component';
import { UserModel } from '../../../Models/user.model';

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
  @Input() user: UserModel | undefined;
  applyToBeTrainer() {
    if (this.loading) return;
    this.loading = true;
    this.trainerApplicationService.applyForTrainer().subscribe({
      next: () => {
        this.loading = false;
        if (this.user) {
          this.user.isAppliedAsTrainer = true;
        }
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
