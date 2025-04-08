import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import {  UserService } from '../../Services/user.service';
import { FormsModule } from '@angular/forms';
import { UserModel } from '../../Models/user.model';

@Component({
  selector: 'app-log-in-page',
  imports: [RouterModule, FormsModule],
  templateUrl: './log-in-page.component.html',
  styleUrl: './log-in-page.component.scss'
})
export class LogInPageComponent {

  
  model = {
    email: '',
    password: '',
    rememberMe: false
  }
  errorMessage = '';

  constructor(
    private userService: UserService,
    private router: Router
  ) { }

  login() {
    if (!this.model.email || !this.model.password) {
      this.errorMessage = "Email cím és jelszó megadása kötelező!";
      return;
    }
    this.userService.login(this.model.email, this.model.password, this.model.rememberMe).subscribe({
      next: (response) => {
        if (response) {
          this.router.navigate(['user']);
        }
        else {
          this.errorMessage = "Helytelen email cím vagy jelszó!";
        }
      },
      error: (error) => {
        this.errorMessage = error.error.message ?? error.message;
      }
    });
  }
}
