import { Component } from '@angular/core';
import { ButtonLoaderComponent } from '../../button-loader/button-loader.component';
import { UserModel } from '../../../Models/user.model';
import { UserService } from '../../../Services/user.service';
import {
  animate,
  keyframes,
  style,
  transition,
  trigger,
} from '@angular/animations';
import { TrainerApplicationService } from '../../../Services/trainer-application.service';
import { NgClass } from '@angular/common';

enum SortDirection {
  Asc = 'asc',
  Desc = 'desc',
}

type SortableProperty = keyof Pick<
  UserModel,
  'name' | 'email' | 'role' | 'phone' | 'appliedForTraining'
>;

@Component({
  selector: 'app-user-cms',
  imports: [ButtonLoaderComponent, NgClass],
  templateUrl: './user-cms.component.html',
  styleUrl: './user-cms.component.scss',
  animations: [
    trigger('errorAnimation', [
      transition(':enter', [
        style({ transform: 'translate(-50%, -100%)', opacity: 0 }),
        animate(
          '500ms ease-out',
          keyframes([
            style({
              transform: 'translate(-50%, -30%)',
              opacity: 0.5,
              offset: 0.3,
            }),
            style({
              transform: 'translate(-50%, 0)',
              opacity: 1,
              offset: 1,
            }),
          ])
        ),
        animate(
          '2000ms',
          keyframes([
            style({
              borderColor: 'var(--danger)',
              boxShadow: '0 0 0 0 rgba(var(--danger-rgb), 0.4)',
              offset: 0,
            }),
            style({
              borderColor: 'rgba(var(--danger-rgb), 0.7)',
              boxShadow: '0 0 0 10px rgba(var(--danger-rgb), 0)',
              offset: 0.7,
            }),
            style({
              borderColor: 'var(--danger)',
              boxShadow: '0 0 0 0 rgba(var(--danger-rgb), 0)',
              offset: 1,
            }),
          ])
        ),
      ]),
    ]),
  ],
})
export class UserCMSComponent {
  toggleShowDeleted() {
    this.showDeleted = !this.showDeleted;
    this.updateDisplayUsers();
  }
  users: UserModel[] = [];
  trainerApplicants: {
    accountId: number;
    id: number;
    acceptedAt: Date | null;
  }[] = [];
  displayUsers: UserModel[] = [];
  searchTerm: string = '';
  deletingUserId: number | null = null;
  loadingUserId: number | null = null;
  bottomError: string | null = null;
  errorMsg: string = '';
  showDeleted: boolean = false;

  sortState: { property: SortableProperty | null; direction: SortDirection } = {
    property: null,
    direction: SortDirection.Asc,
  };

  constructor(
    private userService: UserService,
    private trainerApplicantsService: TrainerApplicationService
  ) {
    this.loadUsers();
  }
  rejectUser(user: UserModel) {
    if (this.loadingUserId) return;
    let id = this.trainerApplicants.find((trn) => trn.accountId == user.id)?.id;
    if (!id) return;
    this.loadingUserId = user.id;
    this.trainerApplicantsService.deleteApplicant(id).subscribe({
      next: (s) => {
        user = s;
        this.loadingUserId = null;
        this.loadUsers();
      },
      error: (err) => {
        this.errorMsg =
          err.error?.message || 'Nem sikerült elutasítani a jelentkezést.';
        this.loadingUserId == null;
      },
    });
  }
  acceptUser(user: UserModel) {
    if (this.loadingUserId) return;
    let asd = this.trainerApplicants.find((trn) => trn.accountId == user.id);

    let id = asd?.id;
    this.loadingUserId = user.id;
    if (!id) return;
    this.trainerApplicantsService.acceptApplicant(id).subscribe({
      next: (s) => {
        this.loadUsers();
        this.loadingUserId = null;
      },
      error: (err) => {
        this.loadingUserId == null;

        this.errorMsg =
          err.error?.message || 'Nem sikerült elfogadni a jelentkezést.';
      },
    });
  }
  private loadUsers() {
    this.userService.getAllUsers().subscribe({
      next: (users) => {
        this.users = users || [];
        this.updateDisplayUsers();
      },
      error: (err) => console.error('Failed to load users:', err),
    });

    this.trainerApplicantsService.getTrainerApplicants().subscribe({
      next: (users) => {
        this.trainerApplicants = users || [];
        this.updateDisplayUsers();
      },
      error: (err) => console.error('Failed to load users:', err),
    });
  }

  search(event: Event) {
    const input = event.target as HTMLInputElement;
    this.searchTerm = input.value.toLowerCase();
    this.updateDisplayUsers();
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
    this.updateDisplayUsers();
  }

  private updateDisplayUsers() {
    this.users.map((usr) => {
      usr.appliedForTraining = !!this.trainerApplicants.find(
        (tA) =>
          tA.accountId === usr.id &&
          tA.acceptedAt === null &&
          usr.role == 'user'
      );
      return usr;
    });
    let filtered = this.users.filter((usr) => usr.isActive || this.showDeleted);
    filtered = filtered.filter(
      (s) =>
        s.email?.toLowerCase().includes(this.searchTerm) ||
        s.name?.toLowerCase().includes(this.searchTerm) ||
        s.phone?.toLowerCase().includes(this.searchTerm) ||
        this.userInHungarian(s).toLowerCase().includes(this.searchTerm)
    );

    if (this.sortState.property) {
      filtered = this.sortusers(
        filtered,
        this.sortState.property,
        this.sortState.direction
      );
    }

    this.displayUsers = filtered;
  }

  private sortusers(
    users: UserModel[],
    property: SortableProperty,
    direction: SortDirection
  ) {
    return [...users].sort((a, b) => {
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

  delete(user: UserModel) {
    this.deletingUserId = user.id;
    this.userService.deleteUser(user).subscribe({
      next: () => {
        this.deletingUserId = null;
        user.isActive = false;
        this.updateDisplayUsers();
      },
      error: (err) => {
        this.errorMsg = err.error?.message || 'Hiba történt törlés közben.';

        this.deletingUserId = null;
      },
    });
  }

  getSortIndicator(property: SortableProperty): string {
    if (this.sortState.property !== property) return '';
    return this.sortState.direction === SortDirection.Asc ? '⬆' : '⬇';
  }
  userInHungarian(user: UserModel): string {
    if (user.role == 'user') return 'Felhasználó';
    if (user.role == 'trainer') return 'Edző';
    if (user.role == 'employee') return 'Dolgozó';
    return user.role || '';
  }
}
