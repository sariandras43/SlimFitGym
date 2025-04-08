import { Component, Input } from '@angular/core';
import { UserService } from '../../../Services/user.service';
import { UserModel } from '../../../Models/user.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-delete-user',
  imports: [],
  templateUrl: './delete-user.component.html',
  styleUrl: './delete-user.component.scss',
})
export class DeleteUserComponent {
  delete() {
    this.userService.deleteUser(this.user).subscribe({
      next: () => {
        this.userService.logout();
        this.router.navigate(['/']);
      },
      error: (err) => {
        console.log(err)
      },
    });
  }
  @Input() user: UserModel = {
    id: 0,
  };
  constructor(private userService: UserService, private router: Router) {}
}
