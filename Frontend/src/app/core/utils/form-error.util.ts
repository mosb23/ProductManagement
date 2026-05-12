import { FormGroup } from '@angular/forms';
import { ApiError } from '../models/api-error.model';

export function applyApiFieldErrors(
  form: FormGroup,
  error: ApiError,
  aliases: Record<string, string> = {}
): void {
  if (!error.fieldErrors) {
    return;
  }

  Object.entries(error.fieldErrors).forEach(([field, messages]) => {
    const controlName = aliases[field] ?? field;
    const control = form.get(controlName);

    if (!control || messages.length === 0) {
      return;
    }

    control.setErrors({
      ...(control.errors ?? {}),
      backend: messages
    });
    control.markAsTouched();
  });
}

export function getBackendErrors(form: FormGroup, controlName: string): string[] {
  const errors = form.get(controlName)?.errors?.['backend'];
  return Array.isArray(errors) ? errors : [];
}
