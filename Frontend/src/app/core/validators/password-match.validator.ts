import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function passwordMatchValidator(
  passwordControlName: string,
  confirmPasswordControlName: string
): ValidatorFn {
  return (formGroup: AbstractControl): ValidationErrors | null => {
    const password = formGroup.get(passwordControlName)?.value;
    const confirmPassword = formGroup.get(confirmPasswordControlName)?.value;

    if (!password || !confirmPassword) {
      return null;
    }

    return password === confirmPassword ? null : { passwordMismatch: true };
  };
}