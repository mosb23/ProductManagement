export interface LoginResponse {
  id: string;
  fullName: string;
  email: string;
  role: string;
  claims: string[];
  token: string;
  expiresAt: string;
}
