import { Component } from '@angular/core';
import { HeroComponent } from '../../Components/hero/hero.component';
import { TrainingModel } from '../../Models/training.model';
import { TrainingService } from '../../Services/training.service';
import { combineLatest, Subscription } from 'rxjs';
import { UserService } from '../../Services/user.service';
import { UserModel } from '../../Models/user.model';
import { FooterComponent } from '../../Components/footer/footer.component';
import Util from '../../utils/util';
import { FormsModule } from '@angular/forms';
import { ButtonLoaderComponent } from '../../Components/button-loader/button-loader.component';

@Component({
  selector: 'app-trainigs-page',
  imports: [HeroComponent, FooterComponent, FormsModule, ButtonLoaderComponent],
  templateUrl: './trainings-page.component.html',
  styleUrl: './trainings-page.component.scss',
})
export class TrainigsPageComponent {
  loading = false;
  searchParam: string | undefined;
  isSearching = false;
  limit = 10;
  offset = 0;
  hasMore = true;
  currentPage = 1;
  trainings: TrainingModel[] = [];
  user: UserModel | undefined;

  constructor(
    private trainingService: TrainingService,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.trainingService.allTrainings$.subscribe({
      next: (t) => {
        if (t) {
          this.trainings = t;
          this.hasMore = t.length === this.limit;
        }
      },
      error: (err) => {
        console.log(err);
      },
    });

    this.userService.loggedInUser$.subscribe((s) => {
      if (s) this.user = s;
    });
  }

  search() {
    this.offset = 0;
    this.currentPage = 1;
    this.isSearching = true;
    this.fetchTrainings();
  }

  nextPage() {
    if (this.hasMore) {
      this.offset += this.limit;
      this.currentPage++;
      this.fetchTrainings();
    }
  }

  previousPage() {
    if (this.offset > 0) {
      this.offset = Math.max(0, this.offset - this.limit);
      this.currentPage = Math.max(1, this.currentPage - 1);
      this.fetchTrainings();
    }
  }

  private fetchTrainings() {
    const params = {
      query: this.searchParam,
      limit: this.limit,
      offset: this.offset,
    };
    this.loading = true;

    this.trainingService.getTrainings(params).subscribe({
      next: () => {
        this.isSearching = false;
        this.loading = false;
      },
      error: () => {
        this.isSearching = false;
        this.loading = false;
      },
    });
  }

  subscribe(training: TrainingModel) {
    this.loading = true;

    this.trainingService.subscribeToTraining(training.id).subscribe({
      next: () => {
        this.loading = false;
      },
      error: (err) => {
        console.log(err);
        this.loading = false;
      },
    });
  }

  unsubscribe(training: TrainingModel) {
    this.loading = true;
    this.trainingService.unsubscribeFromTraining(training.id).subscribe({
      next: () => {
        this.loading = false;
      },
      error: (err) => {
        console.log(err);
        this.loading = false;
      },
    });
  }

  displayDate(training: TrainingModel): string {
    return Util.displayDate(training);
  }
}
