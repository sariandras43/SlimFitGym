import { Directive } from '@angular/core';
import {
  AbstractControl,
  NG_VALIDATORS,
  ValidationErrors,
  Validator,
  ValidatorFn,
} from '@angular/forms';
/** An actor's name can't match the actor's role */
export const passwordValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const password = control.get('password');
  const passwordAgain = control.get('passwordAgain');
  return password && passwordAgain && password.value !== passwordAgain.value
    ? { passwordMismatch: true }
    : null;
};
@Directive({
  selector: '[appPasswordMismatch]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: PasswordMismatchValidatorDirective,
      multi: true,
    },
  ],
  standalone: true,
})
export class PasswordMismatchValidatorDirective implements Validator {
  validate(control: AbstractControl): ValidationErrors | null {
    return passwordValidator(control);
  }
}
