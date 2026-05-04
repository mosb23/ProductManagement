import { ProductStatus } from './product-status.enum';
import { ProductStatusHistory } from './product-status-history.model';

export interface ProductDetails {
  id: number;
  name: string;
  description?: string | null;
  price: number;
  quantity: number;
  status?: ProductStatus | null;
  createdAt?: string;
  updatedAt?: string;
  history?: ProductStatusHistory[] | null;
}
