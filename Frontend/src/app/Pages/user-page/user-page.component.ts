import { Component } from '@angular/core';
import { AccordionSegmentComponent } from '../../Components/accordion-segment/accordion-segment.component';
import { NewPasswordComponent } from '../../Components/CMS/new-password/new-password.component';
import { BasicUserDataComponent } from '../../Components/CMS/basic-user-data/basic-user-data.component';
import { UserModel } from '../../Models/user.model';
import { AuthService } from '../../Services/auth.service';
import { Router } from '@angular/router';
import { PassModel } from '../../Models/pass.model';

@Component({
  selector: 'app-user-page',
  imports: [
    AccordionSegmentComponent,
    NewPasswordComponent,
    BasicUserDataComponent,
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
  constructor(private authService: AuthService, private router: Router) {
    const loggedInUser = localStorage.getItem('loggedInUser');
    const loggedInUserPass = localStorage.getItem('userPass');
    
    if (loggedInUser) {
      this.loggedInUser = JSON.parse(loggedInUser);
    } 
    if (loggedInUserPass) {
      this.loggedInUserPass = JSON.parse(loggedInUserPass);
      
    } 
    
    this.authService.getPass().subscribe({
      next: (response) => {
        if (response) {
          const loggedInUserPass = localStorage.getItem('userPass');
          if (loggedInUserPass) {
            this.loggedInUserPass = JSON.parse(loggedInUserPass);
            console.log(this.loggedInUserPass)
          } 
        }
        
      },
      error: (error) => {
        console.log(error.error.message ?? error.message)
      }
    })
  }

  
}
