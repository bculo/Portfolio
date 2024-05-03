import { Status } from '../../stores/api/generated';

export type StockFilter = {
  page: number;
  take: number;
  symbol: string;
  status: Status;
};
