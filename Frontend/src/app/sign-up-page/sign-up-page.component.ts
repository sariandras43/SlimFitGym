import {  RouterModule } from '@angular/router';
import {FormGroup,FormControl, Validators} from '@angular/forms';
import {ReactiveFormsModule} from '@angular/forms';
import {FormBuilder} from '@angular/forms';
import {Component, inject} from '@angular/core';
@Component({
  selector: 'app-sign-up-page',
  imports: [RouterModule,ReactiveFormsModule],
  templateUrl: './sign-up-page.component.html',
  styleUrl: './sign-up-page.component.scss'
})
export class SignUpPageComponent {
  private formBuilder = inject(FormBuilder);
  profileForm = this.formBuilder.group({
    name: ['', Validators.required],
    email: ['', Validators.required],
    phone: ['', Validators.required],
    password: ['', Validators.required],
    passwordAgain: ['', Validators.required],
    
  });


}
