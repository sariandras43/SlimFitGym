import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UserModel } from '../../Models/user.model';
import {  UserService } from '../../Services/user.service';

@Component({
  selector: 'app-navbar',
  imports: [RouterModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  loggedInUser: UserModel | undefined;
  
  constructor(authService: UserService) {
    
    authService.loggedInUser$.subscribe((user) => {
      this.loggedInUser = user;
    });
  }
}
