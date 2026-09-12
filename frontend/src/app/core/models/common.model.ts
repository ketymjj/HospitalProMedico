export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface PaginationParams {
  pageNumber?: number;
  pageSize?: number;
  search?: string;
  sortBy?: string;
  sortDescending?: boolean;
  [key: string]: unknown;
}

export interface ChartPoint {
  label: string;
  value: number;
}

export interface ApiErrorResponse {
  statusCode: number;
  message: string;
  errors: string[];
  timestamp: string;
}
