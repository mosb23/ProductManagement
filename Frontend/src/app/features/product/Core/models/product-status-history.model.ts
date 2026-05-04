import { ProductStatus } from './product-status.enum';

export interface ProductStatusHistory {
newStatus: any;
createdAt: any;
oldStatus: any;
  id: number;
  productId: number;
  oldStatus: ProductStatus | null;
  newStatus: ProductStatus;
  createdAt: string;
  createdBy?: string | null;
  productName?: string | null;
}
