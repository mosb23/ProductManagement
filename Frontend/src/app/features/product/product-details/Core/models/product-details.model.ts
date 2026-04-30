export interface ProductDetails {
  id: number;
  name: string;
  description?: string | null;
  price: number;
  quantity: number;
  status?: string;
  createdAt?: string;
  updatedAt?: string;
}