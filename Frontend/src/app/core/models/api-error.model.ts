export interface ApiError {
  status: number;
  message: string;
  errors?: string[];
  fieldErrors?: Record<string, string[]>;
}
