import { HttpErrorResponse } from '@angular/common/http';
import { ApiError } from '../models/api-error.model';


export function mapHttpError(error: HttpErrorResponse): ApiError {
  const payload = typeof error.error === 'string'
    ? { message: error.error }
    : error.error ?? {};

  const backendMessage =
    payload.message ||
    payload.title ||
    error.message;

  const { errors: backendErrors, fieldErrors } = normalizeErrors(payload.errors);

  switch (error.status) {
    case 400:
      return {
        status: 400,
        message: backendMessage || 'Invalid request.',
        errors: backendErrors,
        fieldErrors
      };

    case 401:
      return {
        status: 401,
        message: 'Your session has expired. Please login again.'
      };

    case 403:
      return {
        status: 403,
        message: 'Access denied. You do not have permission.'
      };

    case 404:
      return {
        status: 404,
        message: backendMessage || 'The requested resource was not found.'
      };

    case 500:
      return {
        status: 500,
        message: 'Something went wrong on the server. Please try again later.'
      };

    case 0:
      return {
        status: 0,
        message: 'Cannot connect to the server. Please check your connection.'
      };

    default:
      return {
        status: error.status,
        message: backendMessage || 'Unexpected error occurred.',
        errors: backendErrors,
        fieldErrors
      };
  }
}

function normalizeErrors(errors: unknown): { errors?: string[]; fieldErrors?: Record<string, string[]> } {
  if (!errors) {
    return {};
  }

  if (Array.isArray(errors)) {
    const messages = errors.filter((error): error is string => typeof error === 'string');
    return {
      errors: messages,
      fieldErrors: parseFieldErrors(messages)
    };
  }

  if (typeof errors === 'object') {
    const fieldErrors: Record<string, string[]> = {};
    const messages: string[] = [];

    Object.entries(errors as Record<string, unknown>).forEach(([field, value]) => {
      const fieldMessages = Array.isArray(value)
        ? value.filter((item): item is string => typeof item === 'string')
        : typeof value === 'string'
          ? [value]
          : [];

      if (fieldMessages.length > 0) {
        fieldErrors[normalizeFieldName(field)] = fieldMessages;
        messages.push(...fieldMessages);
      }
    });

    return {
      errors: messages,
      fieldErrors: Object.keys(fieldErrors).length > 0 ? fieldErrors : undefined
    };
  }

  return {};
}

function parseFieldErrors(messages: string[]): Record<string, string[]> | undefined {
  const fieldErrors: Record<string, string[]> = {};

  messages.forEach(message => {
    const separatorIndex = message.indexOf(':');

    if (separatorIndex <= 0) {
      return;
    }

    const field = normalizeFieldName(message.slice(0, separatorIndex));
    const fieldMessage = message.slice(separatorIndex + 1).trim();

    if (!field || !fieldMessage) {
      return;
    }

    fieldErrors[field] = [...(fieldErrors[field] ?? []), fieldMessage];
  });

  return Object.keys(fieldErrors).length > 0 ? fieldErrors : undefined;
}

function normalizeFieldName(field: string): string {
  const lastSegment = field.split('.').filter(Boolean).pop() ?? field;
  return lastSegment.charAt(0).toLowerCase() + lastSegment.slice(1);
}
