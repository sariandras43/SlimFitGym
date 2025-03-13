import { Component, Input } from '@angular/core';
import { UserModel } from '../../../Models/user.model';
import { FormsModule } from '@angular/forms';
import { PassModel } from '../../../Models/pass.model';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../../Services/user.service';

@Component({
  selector: 'app-basic-user-data',
  imports: [FormsModule, RouterLink],
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
    const user =
    localStorage.getItem('loggedInUser') ||
    sessionStorage.getItem('loggedInUser');
    if (!user) {
      return;
    }
    const parsedUser: UserModel = JSON.parse(user);
    let updateUserData: UserModel = {id: parsedUser.id};
    
    if (!(updateUserData.email == '' || this.user.email == parsedUser.email)) {
      updateUserData.email = this.user.email;
    }
    if (!(updateUserData.imageUrl == '' || this.user.imageUrl == parsedUser.imageUrl)) {
      updateUserData.imageUrl = this.user.imageUrl;
    }
    if (!(updateUserData.name == '' || this.user.name == parsedUser.name)) {
      updateUserData.name = this.user.name;
    }
    if (!(updateUserData.phone == '' || this.user.phone == parsedUser.phone)) {
      updateUserData.phone = this.user.phone;
    }
    this.userService.updateUser(updateUserData).subscribe({
      next: (response) => {
        if (response) {
          console.log(response)
        }
        else {
          console.log( "Helytelen email cím vagy jelszó!");
        }
      },
      error: (error) => {
        console.log(error.error.message ?? error.message);
      }
    });

    
  }
}
