import { Component, Input } from '@angular/core';
import { UserModel } from '../../../Models/user.model';
import { FormsModule } from '@angular/forms';
import { PassModel } from '../../../Models/pass.model';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-basic-user-data',
  imports: [FormsModule, RouterLink],
  templateUrl: './basic-user-data.component.html',
  styleUrl: './basic-user-data.component.scss',
})
export class BasicUserDataComponent {
  @Input() user: UserModel ={
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
  constructor() {

  }
}
