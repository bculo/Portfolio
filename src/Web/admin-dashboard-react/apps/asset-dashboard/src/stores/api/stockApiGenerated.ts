/* eslint-disable @typescript-eslint/no-explicit-any */
import { stockApi as api } from './stockApiSlice';
const injectedRtkApi = api.injectEndpoints({
  endpoints: (build) => ({
    evictAll: build.mutation<EvictAllApiResponse, EvictAllApiArg>({
      query: () => ({ url: `/api/v1/Cache/EvictAll`, method: 'DELETE' }),
    }),
    createStock: build.mutation<CreateStockApiResponse, CreateStockApiArg>({
      query: (queryArg) => ({
        url: `/api/v1/Stock/Create`,
        method: 'POST',
        body: queryArg,
      }),
    }),
    getStock: build.query<GetStockApiResponse, GetStockApiArg>({
      query: (queryArg) => ({ url: `/api/v1/Stock/Single/${queryArg}` }),
    }),
    getStocks: build.query<GetStocksApiResponse, GetStocksApiArg>({
      query: () => ({ url: `/api/v1/Stock/All` }),
    }),
    filterStocks: build.query<FilterStocksApiResponse, FilterStocksApiArg>({
      query: (queryArg) => ({
        url: `/api/v1/Stock/Filter`,
        params: {
          'Symbol.Value': queryArg['Symbol.Value'],
          'PriceGreaterThan.Value': queryArg['PriceGreaterThan.Value'],
          'PriceLessThan.Value': queryArg['PriceLessThan.Value'],
          'ActivityStatus.Value': queryArg['ActivityStatus.Value'],
          'NotOlderThan.Value': queryArg['NotOlderThan.Value'],
          'SortBy.PropertyName': queryArg['SortBy.PropertyName'],
          'SortBy.Direction': queryArg['SortBy.Direction'],
          Page: queryArg.page,
          Take: queryArg.take,
        },
      }),
    }),
    changeStockStatus: build.mutation<
      ChangeStockStatusApiResponse,
      ChangeStockStatusApiArg
    >({
      query: (queryArg) => ({
        url: `/api/v1/Stock/ChangeStatus`,
        method: 'PUT',
        body: queryArg,
      }),
    }),
  }),
  overrideExisting: false,
});
export { injectedRtkApi as generated };
export type EvictAllApiResponse = unknown;
export type EvictAllApiArg = void;
export type CreateStockApiResponse = /** status 200 Success */ string;
export type CreateStockApiArg = CreateStock;
export type GetStockApiResponse =
  /** status 200 Success */ GetStockByIdResponse;
export type GetStockApiArg = string;
export type GetStocksApiResponse =
  /** status 200 Success */ GetStocksResponse[];
export type GetStocksApiArg = void;
export type FilterStocksApiResponse =
  /** status 200 Success */ PageResultDtoFilterStockResponseItem;
export type FilterStocksApiArg = {
  'Symbol.Value'?: string;
  'PriceGreaterThan.Value'?: number;
  'PriceLessThan.Value'?: number;
  'ActivityStatus.Value'?: Status;
  'NotOlderThan.Value'?: string;
  'SortBy.PropertyName'?: string;
  'SortBy.Direction'?: SortDirection;
  page?: number;
  take?: number;
};
export type ChangeStockStatusApiResponse = /** status 204 No Content */ void;
export type ChangeStockStatusApiArg = ChangeStockStatus;
export type ProblemDetails = {
  type?: string | null;
  title?: string | null;
  status?: number | null;
  detail?: string | null;
  instance?: string | null;
  [key: string]: any;
};
export type HttpValidationProblemDetails = ProblemDetails & {
  errors?: {
    [key: string]: string[];
  };
  [key: string]: any;
};
export type CreateStock = {
  symbol?: string;
};
export type StockPriceResultDto = {
  id?: string;
  symbol?: string;
  price?: number | null;
  isActive?: boolean;
  lastPriceUpdate?: string | null;
  created?: string;
};
export type GetStockByIdResponse = StockPriceResultDto;
export type GetStocksResponse = StockPriceResultDto;
export type FilterStockResponseItem = StockPriceResultDto;
export type PageResultDtoFilterStockResponseItem = {
  totalCount?: number;
  items?: FilterStockResponseItem[];
};
export type Status = 1 | 2 | 999;
export type SortDirection = 0 | 1;
export type ChangeStockStatus = {
  id?: string;
};
export const {
  useEvictAllMutation,
  useCreateStockMutation,
  useGetStockQuery,
  useLazyGetStockQuery,
  useGetStocksQuery,
  useLazyGetStocksQuery,
  useFilterStocksQuery,
  useLazyFilterStocksQuery,
  useChangeStockStatusMutation,
} = injectedRtkApi;
