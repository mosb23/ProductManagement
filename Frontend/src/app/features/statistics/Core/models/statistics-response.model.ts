export interface StatisticsResponse {
  products: ProductStatisticsResponse;
  statusChanges: StatusChangeStatisticsResponse;
  users: UserStatisticsResponse;
}

export interface ProductStatisticsResponse {
  total: number;
  available: number;
  outOfStock: number;
  discontinued: number;
}

export interface StatusChangeStatisticsResponse {
  total: number;
}

export interface UserStatisticsResponse {
  total: number;
  active: number;
  inactive: number;
}