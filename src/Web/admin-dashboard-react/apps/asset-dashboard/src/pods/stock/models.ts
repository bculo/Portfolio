import { Status } from '../../stores/api/stockApiGenerated';

export type StockFilter = {
  page: number;
  take: number;
  symbol: string;
  status: Status;
};
