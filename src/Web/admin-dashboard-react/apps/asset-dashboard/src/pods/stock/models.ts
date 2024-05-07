import { Status } from '../../stores/stock/stockApiGenerated';

export type StockFilter = {
  page: number;
  take: number;
  symbol: string;
  status: Status;
};
