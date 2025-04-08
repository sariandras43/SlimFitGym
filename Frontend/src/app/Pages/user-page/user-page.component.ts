import { Component } from '@angular/core';
import { AccordionSegmentComponent } from '../../Components/accordion-segment/accordion-segment.component';
import { NewPasswordComponent } from '../../Components/CMS/new-password/new-password.component';
import { BasicUserDataComponent } from '../../Components/CMS/basic-user-data/basic-user-data.component';
import { UserModel } from '../../Models/user.model';
import {  UserService } from '../../Services/user.service';
import { Router, RouterOutlet } from '@angular/router';
import { PassModel } from '../../Models/pass.model';
import { SidenavComponent } from "../../Components/sidenav/sidenav.component";

@Component({
  selector: 'app-user-page',
  imports: [
    NewPasswordComponent,
    BasicUserDataComponent,
    RouterOutlet,
    SidenavComponent
],
  templateUrl: './user-page.component.html',
  styleUrl: './user-page.component.scss',
})
export class UserPageComponent {
  loggedInUser: UserModel | undefined = undefined;
  loggedInUserPass: PassModel | undefined = undefined;
  /**
   *
   */
  constructor(private authService: UserService, private router: Router) {
    const loggedInUser = localStorage.getItem('loggedInUser');
    const loggedInUserPass = localStorage.getItem('userPass');
    
    if (loggedInUser) {
      this.loggedInUser = JSON.parse(loggedInUser);
    } 
    if (loggedInUserPass) {
      this.loggedInUserPass = JSON.parse(loggedInUserPass);
      
    } 
    
    this.authService.getPass()

    authService.loggedInUser$.subscribe((res)=>{ this.loggedInUser = res})
    authService.loggedInUserPass$.subscribe((res)=>{ this.loggedInUserPass = res})
  }

  
}
