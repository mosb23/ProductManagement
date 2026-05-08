import { AbstractControl, ValidationErrors } from '@angular/forms';

export function passwordStrengthValidator(control: AbstractControl): ValidationErrors | null {
  const value = control.value as string;

  if (!value) {
    return null;
  }

  const hasUpper = /[A-Z]/.test(value);
  const hasLower = /[a-z]/.test(value);
  const hasNumber = /\d/.test(value);
  const hasSymbol = /[^A-Za-z0-9]/.test(value);

  const isValid = hasUpper && hasLower && hasNumber && hasSymbol;

  return isValid ? null : { passwordStrength: true };
}