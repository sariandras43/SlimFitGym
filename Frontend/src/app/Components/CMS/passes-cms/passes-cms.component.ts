import { Component } from '@angular/core';
import { PassModel } from '../../../Models/pass.model';
import { PassService } from '../../../Services/pass.service';
import { FormsModule } from '@angular/forms';
import { ButtonLoaderComponent } from '../../button-loader/button-loader.component';

enum SortDirection {
  Asc = 'asc',
  Desc = 'desc',
}

type SortableProperty = keyof Pick<PassModel, 'name' | 'price'>;

@Component({
  selector: 'app-passes-cms',
  standalone: true,
  imports: [FormsModule, ButtonLoaderComponent],
  templateUrl: './passes-cms.component.html',
  styleUrls: ['./passes-cms.component.scss'],
})
export class PassesCMSComponent {
  passes: PassModel[] = [];
  displayPasses: PassModel[] = [];
  selectedPass: PassModel | undefined;
  searchTerm: string = '';
  formSubmitted = false;
  nameError = false;
  priceError = false;
  isSubmitting = false;
  showDeleted = false;

  sortState: { property: SortableProperty | null; direction: SortDirection } = {
    property: null,
    direction: SortDirection.Asc,
  };

  constructor(private passService: PassService) {
    this.loadPasses();
  }

  private loadPasses() {
    this.passService.getPassesAll().subscribe({
        next: (passes) => {
          this.passes = passes || [];
          this.updateDisplayPasses();
        },
        error: (err) => console.error('Failed to load passes:', err),
      });
  }

  search(event: Event) {
    const input = event.target as HTMLInputElement;
    this.searchTerm = input.value.toLowerCase();
    this.updateDisplayPasses();
  }

  sortBy(property: SortableProperty) {
    if (this.sortState.property === property) {
      this.sortState.direction =
        this.sortState.direction === SortDirection.Asc
          ? SortDirection.Desc
          : SortDirection.Asc;
    } else {
      this.sortState = { property, direction: SortDirection.Asc };
    }
    this.updateDisplayPasses();
  }

  private updateDisplayPasses() {
    let filtered = this.passes.filter(
      (pass) =>
        (pass.isActive || this.showDeleted) &&
        (pass.name.toLowerCase().includes(this.searchTerm) ||
          pass.benefits.some((b) => b.toLowerCase().includes(this.searchTerm)))
    );

    if (this.sortState.property) {
      filtered = this.sortPasses(
        filtered,
        this.sortState.property,
        this.sortState.direction
      );
    }

    this.displayPasses = filtered;
  }

  private sortPasses(
    passes: PassModel[],
    property: SortableProperty,
    direction: SortDirection
  ) {
    return [...passes].sort((a, b) => {
      const aVal = a[property];
      const bVal = b[property];

      if (typeof aVal === 'number' && typeof bVal === 'number') {
        return direction === SortDirection.Asc ? aVal - bVal : bVal - aVal;
      }
      return (
        String(aVal).localeCompare(String(bVal)) *
        (direction === SortDirection.Asc ? 1 : -1)
      );
    });
  }

  delete(pass: PassModel) {
    if (!pass.isActive) return;

    this.passService.deletePass(pass).subscribe({
      next: () => {
        pass.isActive = false;
        this.updateDisplayPasses();
      },
      error: (err) => console.error('Delete failed:', err),
    });
  }

  save() {
    if (this.isSubmitting || !this.selectedPass) return;

    this.formSubmitted = true;
    this.nameError = !this.selectedPass.name.trim();
    this.priceError = !this.selectedPass.price;

    if (this.nameError || this.priceError) return;

    this.isSubmitting = true;
    const { isActive, ...payload } = this.selectedPass;


    this.passService.savePass(payload).subscribe({
      next: (updated) => {
        const index = this.passes.findIndex((p) => p.id === updated.id);
        if (index > -1) {
          this.passes[index] = updated;
        } else {
          this.passes.unshift(updated);
        }
        this.selectedPass = undefined;
        this.isSubmitting = false;
        this.updateDisplayPasses();
      },
      error: (err) => {
        console.error(err);
        this.isSubmitting = false;
      },
    });
  }

  addBenefit() {
    if (this.selectedPass) {
      this.selectedPass.benefits = [...this.selectedPass.benefits, ''];
    }
  }

  removeBenefit(index: number) {
    if (this.selectedPass) {
      this.selectedPass.benefits = this.selectedPass.benefits.filter(
        (_, i) => i !== index
      );
    }
  }

  modalOpen(pass?: PassModel) {
    this.selectedPass = pass
      ? { ...pass, benefits: [...pass.benefits] }
      : {
          id: -1,
          name: '',
          maxEntries: undefined,
          days: undefined,
          price: 0,
          isActive: true,
          isHighlighted: false,
          benefits: [],
          validTo: new Date().toISOString().split('T')[0],
          remainingEntries: undefined,
        };
  }

  toggleShowDeleted() {
    this.showDeleted = !this.showDeleted;
    this.updateDisplayPasses();
  }

  getSortIndicator(property: SortableProperty): string {
    if (this.sortState.property !== property) return '';
    return this.sortState.direction === SortDirection.Asc ? '⬆' : '⬇';
  }
}
