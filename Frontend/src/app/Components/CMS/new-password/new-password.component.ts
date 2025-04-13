import { Component, Input } from '@angular/core';
import { AccordionSegmentComponent } from '../../accordion-segment/accordion-segment.component';
import { UserService } from '../../../Services/user.service';
import { FormsModule } from '@angular/forms';
import { UserModel } from '../../../Models/user.model';

@Component({
  selector: 'app-new-password',
  imports: [AccordionSegmentComponent, FormsModule],
  templateUrl: './new-password.component.html',
  styleUrl: './new-password.component.scss',
})
export class NewPasswordComponent {
  password: string = '';
  passwordAgain: string = '';
  @Input() user: UserModel = {
    id: 0,
  };
  constructor(private userService: UserService) {}
  newPassword() {
    if (this.passwordAgain == this.password) {
      const userModel: UserModel = {
        id: this.user.id,
        newPassword: this.password,
      };
      this.userService.updateUser(userModel).subscribe({
        next: () => {},
        error: (error) => {
          console.log(error.error.message ?? error.message);
        },
      });
      this.password = '';
      this.passwordAgain = '';
    }
  }
}
