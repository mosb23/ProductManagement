import { ProductStatus } from './product-status.enum';

export interface ProductListItem {
  id: number;
  name: string;
  description?: string | null;
  price: number;
  quantity: number;
  status?: ProductStatus | null;
  createdAt?: string;
}
