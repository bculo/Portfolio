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
export type CreateStock = {
  symbol?: string | null;
};
export type GetStockByIdResponse = {
  id?: string | null;
  symbol?: string | null;
  price?: number | null;
  isActive?: boolean;
  lastPriceUpdate?: string | null;
  created?: string;
};
export type GetStocksResponse = {
  id?: string | null;
  symbol?: string | null;
  price?: number | null;
  isActive?: boolean;
  lastPriceUpdate?: string | null;
  created?: string;
};
export type FilterStockResponseItem = {
  id?: string | null;
  symbol?: string | null;
  price?: number | null;
  isActive?: boolean;
  lastPriceUpdate?: string | null;
  created?: string;
};
export type PageResultDtoFilterStockResponseItem = {
  totalCount?: number;
  items?: FilterStockResponseItem[] | null;
};
export type Status = 1 | 2 | 999;
export type ChangeStockStatus = {
  id?: string | null;
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
