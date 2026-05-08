export interface CreateUserRequest {
  fullName: string;
  email: string;
  password: string;
  confirmPassword: string;
  role: string;
}