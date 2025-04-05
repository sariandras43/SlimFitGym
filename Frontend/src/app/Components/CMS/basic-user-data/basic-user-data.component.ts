import { Component, Input } from '@angular/core';
import { UserModel } from '../../../Models/user.model';
import { FormsModule } from '@angular/forms';
import { PassModel } from '../../../Models/pass.model';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../../Services/user.service';
import { ButtonLoaderComponent } from '../../button-loader/button-loader.component';
import { NgClass } from '@angular/common';
import { NewPasswordComponent } from "../new-password/new-password.component";
import { DeleteUserComponent } from "../delete-user/delete-user.component";
import { ApplyForTrainerComponent } from "../apply-for-trainer/apply-for-trainer.component";

@Component({
  selector: 'app-basic-user-data',
  imports: [FormsModule, RouterLink, ButtonLoaderComponent, NgClass, NewPasswordComponent, DeleteUserComponent, ApplyForTrainerComponent],
  templateUrl: './basic-user-data.component.html',
  styleUrl: './basic-user-data.component.scss',
})
export class BasicUserDataComponent {
  user: UserModel = {
    id: 0,
  };

  inputChanged = false;
  updateLoading: Boolean = false;
  deleteImageLoading: Boolean = false;
  canModify: Boolean = false;
  @Input() loggedInUserPass: PassModel | undefined;

  originalUser: UserModel = {id:0};

  constructor(private userService: UserService, private router: Router) {
    

   
    this.userService.getPass();

    userService.loggedInUser$.subscribe((res) => {
      if (res) {
        this.user = res;
        this.originalUser = {...res};
      }
    });
    userService.loggedInUserPass$.subscribe((res) => {
      this.loggedInUserPass = res;
    });
  }
  formChanged() {
    this.canModify = this.changedUserValue !== null;
  }
  public get changedUserValue() {
    let hasChanged = false;
    let updateUserData: UserModel = { id: this.originalUser.id };

    if (!(this.user.email == '' || this.user.email == this.originalUser.email)) {
      updateUserData.email = this.user.email;
      hasChanged = true;
    }
    if (this.user.image != undefined && this.user.image != '') {
      updateUserData.image = this.user.image;
      hasChanged = true;
    }
    if (!(this.user.name == '' || this.user.name == this.originalUser.name)) {
      updateUserData.name = this.user.name;
      hasChanged = true;
    }
    if (!(this.user.phone == '' || this.user.phone == this.originalUser.phone)) {
      updateUserData.phone = this.user.phone;
      hasChanged = true;
    }

    return hasChanged ? updateUserData : null;
  }

  logout() {
    this.userService.logout();

    this.router.navigate(['/']);
  }
  updateUser() {
    let updateUser = this.changedUserValue;
    if (updateUser) {
      this.updateLoading = true;
      this.userService.updateUser(updateUser).subscribe({
        next: (response) => {
          this.updateLoading = false;
          this.user = this.originalUser;
          this.formChanged();
        },
        error: (error) => {
          this.updateLoading = false;

          console.log(error.error.message ?? error.message);
        },
      });
    }
  }
  imgDelete() {
    this.deleteImageLoading = true;

    let updateUserData: UserModel = { id: this.originalUser.id, image: '' };
    this.userService.updateUser(updateUserData).subscribe({
      next: (response) => {
        this.deleteImageLoading = false;
      },
      error: (error) => {
        this.deleteImageLoading = false;

        console.log(error.error.message ?? error.message);
      },
    });
  }
  imageChanged(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = (e: any) => {
        this.user.imageUrl = e.target.result;
        const res = reader.result?.toString();
        if (res) {
          this.user.image = res;
          this.formChanged();
        }
      };
    }
  }
}
