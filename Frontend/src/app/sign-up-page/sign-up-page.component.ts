import { RouterModule } from '@angular/router';
import { FormGroup, FormControl, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-sign-up-page',
  imports: [RouterModule, ReactiveFormsModule, CommonModule],
  templateUrl: './sign-up-page.component.html',
  styleUrl: './sign-up-page.component.scss',
})
export class SignUpPageComponent {

  passwordMatch(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password');
    const passwordAgain = control.get('passwordAgain');

    if (password && passwordAgain && password.value !== passwordAgain.value) {
      return { passwordMismatch: true };
    }

    return null;
  }
  private formBuilder = inject(FormBuilder);
  profileForm = this.formBuilder.group({
    name: ['', [Validators.required, Validators.minLength(4)]],
    email: [
      '',
      [
        Validators.required,
        Validators.pattern(/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/),
      ],
    ],
    phone: ['', [Validators.required, Validators.pattern(/^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/)]],
    password: ['', [Validators.required, Validators.pattern(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/)] ],
    passwordAgain: ['', Validators.required],
  } , { validators: this.passwordMatch });
  get name() {
    return this.profileForm.get('name');
  }
  get email() {
    return this.profileForm.get('email');
  }
  
  get phone() {
    return this.profileForm.get('phone');
  }
  get password() {
    return this.profileForm.get('password');
  }
  get passwordAgain() {
    return this.profileForm.get('passwordAgain');
  }

 
}
