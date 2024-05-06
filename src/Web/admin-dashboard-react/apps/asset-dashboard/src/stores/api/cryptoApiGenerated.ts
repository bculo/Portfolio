/* eslint-disable @typescript-eslint/no-explicit-any */
import { cryptoApi as api } from './cryptoApiSlice';
const injectedRtkApi = api.injectEndpoints({
  endpoints: (build) => ({
    addNew: build.mutation<AddNewApiResponse, AddNewApiArg>({
      query: (queryArg) => ({
        url: `/api/v1/Crypto/AddNew`,
        method: 'POST',
        body: queryArg,
      }),
    }),
    addNewWithDelay: build.mutation<
      AddNewWithDelayApiResponse,
      AddNewWithDelayApiArg
    >({
      query: (queryArg) => ({
        url: `/api/v1/Crypto/AddNewWithDelay`,
        method: 'POST',
        body: queryArg,
      }),
    }),
    undoAddNewDelay: build.mutation<
      UndoAddNewDelayApiResponse,
      UndoAddNewDelayApiArg
    >({
      query: (queryArg) => ({
        url: `/api/v1/Crypto/UndoAddNewDelay`,
        method: 'POST',
        body: queryArg,
      }),
    }),
    updateInfo: build.mutation<UpdateInfoApiResponse, UpdateInfoApiArg>({
      query: (queryArg) => ({
        url: `/api/v1/Crypto/UpdateInfo`,
        method: 'PUT',
        body: queryArg,
      }),
    }),
    updatePrice: build.mutation<UpdatePriceApiResponse, UpdatePriceApiArg>({
      query: (queryArg) => ({
        url: `/api/v1/Crypto/UpdatePrice`,
        method: 'PUT',
        body: queryArg,
      }),
    }),
    updateAllPrices: build.mutation<
      UpdateAllPricesApiResponse,
      UpdateAllPricesApiArg
    >({
      query: () => ({ url: `/api/v1/Crypto/UpdateAllPrices`, method: 'PUT' }),
    }),
    fetchPage: build.query<FetchPageApiResponse, FetchPageApiArg>({
      query: (queryArg) => ({
        url: `/api/v1/Crypto/FetchPage`,
        params: { Page: queryArg.page, Take: queryArg.take },
      }),
    }),
    single: build.query<SingleApiResponse, SingleApiArg>({
      query: (queryArg) => ({ url: `/api/v1/Crypto/Single/${queryArg}` }),
    }),
    getPriceHistory: build.query<
      GetPriceHistoryApiResponse,
      GetPriceHistoryApiArg
    >({
      query: (queryArg) => ({
        url: `/api/v1/Crypto/GetPriceHistory/${queryArg}`,
      }),
    }),
    getMostPopular: build.query<
      GetMostPopularApiResponse,
      GetMostPopularApiArg
    >({
      query: (queryArg) => ({
        url: `/api/v1/Crypto/GetMostPopular`,
        params: { Take: queryArg },
      }),
    }),
    assemblyVersion: build.query<
      AssemblyVersionApiResponse,
      AssemblyVersionApiArg
    >({
      query: () => ({ url: `/api/v1/Info/AssemblyVersion` }),
    }),
  }),
  overrideExisting: false,
});
export { injectedRtkApi as cryptoApiGenerated };
export type AddNewApiResponse = /** status 204 No Content */ void;
export type AddNewApiArg = AddNewCommand;
export type AddNewWithDelayApiResponse = /** status 200 Success */ string;
export type AddNewWithDelayApiArg = AddNewWithDelayCommand;
export type UndoAddNewDelayApiResponse = /** status 204 No Content */ void;
export type UndoAddNewDelayApiArg = UndoNewWithDelayCommand;
export type UpdateInfoApiResponse = /** status 204 No Content */ void;
export type UpdateInfoApiArg = UpdateInfoCommand;
export type UpdatePriceApiResponse = /** status 204 No Content */ void;
export type UpdatePriceApiArg = UpdatePriceCommand;
export type UpdateAllPricesApiResponse = /** status 204 No Content */ void;
export type UpdateAllPricesApiArg = void;
export type FetchPageApiResponse =
  /** status 200 Success */ FetchPageResponseDto[];
export type FetchPageApiArg = {
  page?: number;
  take?: number;
};
export type SingleApiResponse =
  /** status 200 Success */ FetchSingleResponseDto;
export type SingleApiArg = string;
export type GetPriceHistoryApiResponse =
  /** status 200 Success */ PriceHistoryDto[];
export type GetPriceHistoryApiArg = string;
export type GetMostPopularApiResponse =
  /** status 200 Success */ GetMostPopularResponse[];
export type GetMostPopularApiArg = number;
export type AssemblyVersionApiResponse = unknown;
export type AssemblyVersionApiArg = void;
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
export type AddNewCommand = {
  symbol?: string;
  correlationId?: string | null;
};
export type AddNewWithDelayCommand = {
  symbol?: string;
};
export type UndoNewWithDelayCommand = {
  temporaryId?: string;
};
export type UpdateInfoCommand = {
  symbol?: string;
};
export type UpdatePriceCommand = {
  symbol?: string;
};
export type FetchPageResponseDto = {
  id?: string;
  symbol?: string;
  name?: string;
  website?: string;
  sourceCode?: string;
  price?: number;
};
export type FetchSingleResponseDto = {
  id?: string;
  symbol?: string;
  name?: string;
  price?: number;
};
export type PriceHistoryDto = {
  cryptoId?: string;
  symbol?: string;
  name?: string;
  website?: string;
  sourceCode?: string;
  timeBucket?: string;
  avgPrice?: number;
  maxPrice?: number;
  minPrice?: number;
  lastPrice?: number;
};
export type GetMostPopularResponse = {
  symbol?: string;
  count?: number;
};
export const {
  useAddNewMutation,
  useAddNewWithDelayMutation,
  useUndoAddNewDelayMutation,
  useUpdateInfoMutation,
  useUpdatePriceMutation,
  useUpdateAllPricesMutation,
  useFetchPageQuery,
  useLazyFetchPageQuery,
  useSingleQuery,
  useLazySingleQuery,
  useGetPriceHistoryQuery,
  useLazyGetPriceHistoryQuery,
  useGetMostPopularQuery,
  useLazyGetMostPopularQuery,
  useAssemblyVersionQuery,
  useLazyAssemblyVersionQuery,
} = injectedRtkApi;
