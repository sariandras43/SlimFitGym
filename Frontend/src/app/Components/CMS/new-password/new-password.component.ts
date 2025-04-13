import { Component, Input } from '@angular/core';
import { AccordionSegmentComponent } from '../../accordion-segment/accordion-segment.component';
import { UserService } from '../../../Services/user.service';
import { FormsModule } from '@angular/forms';
import { UserModel } from '../../../Models/user.model';
import { ButtonLoaderComponent } from "../../button-loader/button-loader.component";
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-new-password',
  imports: [FormsModule, ButtonLoaderComponent, NgClass ],
  templateUrl: './new-password.component.html',
  styleUrl: './new-password.component.scss',
})
export class NewPasswordComponent {
  loading = false;
  errorMsg = '';
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
      this.loading = true;
      this.userService.updateUser(userModel).subscribe({
        
        next: () => {
          this.loading = false;
          this.password = '';
          this.passwordAgain = '';
        },
        error: (error) => {
          this.loading = false;
          this.errorMsg = error.error.message ?? error.message;
          console.log(error.error.message ?? error.message);
        },
      });
    }
  }
}
