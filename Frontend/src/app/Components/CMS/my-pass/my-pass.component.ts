import { Component } from '@angular/core';
import { UserService } from '../../../Services/user.service';
import { PassModel } from '../../../Models/pass.model';
import { ButtonLoaderComponent } from "../../button-loader/button-loader.component";
import { RouterLink } from '@angular/router';
import { CurrencyPipe, DatePipe, NgClass } from '@angular/common';


@Component({
  selector: 'app-my-pass',
  imports: [ButtonLoaderComponent, RouterLink, CurrencyPipe, DatePipe, NgClass],
  templateUrl: './my-pass.component.html',
  styleUrl: './my-pass.component.scss'
})
export class MyPassComponent {
  pass: PassModel | undefined;
  isLoading = true;

  constructor(private userService: UserService) {
    this.userService.loggedInUserPass$.subscribe(pass => {
      this.pass = pass;
      
      
      this.isLoading = false;
    });
  }

  ngOnInit() {
    this.userService.getPass();
  }
}
