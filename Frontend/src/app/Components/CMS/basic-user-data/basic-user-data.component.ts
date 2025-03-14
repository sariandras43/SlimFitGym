import { Component, Input } from '@angular/core';
import { UserModel } from '../../../Models/user.model';
import { FormsModule } from '@angular/forms';
import { PassModel } from '../../../Models/pass.model';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../../Services/user.service';
import { ButtonLoaderComponent } from "../../button-loader/button-loader.component";

@Component({
  selector: 'app-basic-user-data',
  imports: [FormsModule, RouterLink, ButtonLoaderComponent],
  templateUrl: './basic-user-data.component.html',
  styleUrl: './basic-user-data.component.scss',
})
export class BasicUserDataComponent {
  @Input() user: UserModel = {
    email: '',
    id: 0,
    imageUrl: '',
    name: '',
    phone: '',
    role: '',
    token: '',
    validTo: '',
  };
  updateLoading:  Boolean = false;
  deleteImageLoading: Boolean = false;
  @Input() loggedInUserPass: PassModel | undefined;

  /**
   *
   */
  constructor(private userService: UserService, private router: Router) {}
  logout() {
    this.userService.logout();
    this.router.navigate(['/']);
  }
  
  updateUser() {
    this.updateLoading = true;
    const user =
      localStorage.getItem('loggedInUser') ||
      sessionStorage.getItem('loggedInUser');
    if (!user) {
      return;
    }
    const parsedUser: UserModel = JSON.parse(user);
    let updateUserData: UserModel = { id: parsedUser.id };

    if (!(this.user.email == '' || this.user.email == parsedUser.email)) {
      updateUserData.email = this.user.email;
    }
    if (!(this.user.image == undefined)) {
      updateUserData.image = this.user.image;
    }
    if (!(this.user.name == '' || this.user.name == parsedUser.name)) {
      updateUserData.name = this.user.name;
    }
    if (!(this.user.phone == '' || this.user.phone == parsedUser.phone)) {
      updateUserData.phone = this.user.phone;
    }
    this.userService.updateUser(updateUserData).subscribe({
      next: (response) => {
        this.updateLoading = false;
        console.log(response);
      },
      error: (error) => {
        this.updateLoading = false;

        console.log(error.error.message ?? error.message);
      },
    });
  }
  imgDelete(){
    this.deleteImageLoading = true;
    const user =
      localStorage.getItem('loggedInUser') ||
      sessionStorage.getItem('loggedInUser');
    if (!user) {
      return;
    }
    
    const parsedUser: UserModel = JSON.parse(user);
    let updateUserData: UserModel = { id: parsedUser.id, image: '' };
    this.userService.updateUser(updateUserData).subscribe({
      next: (response) => {
        this.deleteImageLoading = false;
        console.log(response);
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
        }
        console.log(this.user.image);
      };
    }
  }
}
