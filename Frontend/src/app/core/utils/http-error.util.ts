import { HttpErrorResponse } from '@angular/common/http';
import { ApiError } from '../models/api-error.model';


export function mapHttpError(error: HttpErrorResponse): ApiError {
  const backendMessage =
    error.error?.message ||
    error.error?.title ||
    error.message;

  const backendErrors =
    Array.isArray(error.error?.errors)
      ? error.error.errors
      : undefined;

  switch (error.status) {
    case 400:
      return {
        status: 400,
        message: backendMessage || 'Invalid request.',
        errors: backendErrors
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
        message: backendMessage || 'Unexpected error occurred.'
      };
  }
}