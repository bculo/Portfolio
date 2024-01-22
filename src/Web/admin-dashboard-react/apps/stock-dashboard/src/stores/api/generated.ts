import { stockApi as api } from './stock-api-slice';
const injectedRtkApi = api.injectEndpoints({
  endpoints: (build) => ({
    addNew: build.mutation<AddNewApiResponse, AddNewApiArg>({
      query: (queryArg) => ({
        url: `/api/v1/Stock/AddNew`,
        method: 'POST',
        body: queryArg.addNewStockCommand,
      }),
    }),
    getSingle: build.query<GetSingleApiResponse, GetSingleApiArg>({
      query: (queryArg) => ({
        url: `/api/v1/Stock/Single/${queryArg['symbol']}`,
      }),
    }),
    getAll: build.query<GetAllApiResponse, GetAllApiArg>({
      query: () => ({ url: `/api/v1/Stock/GetAll` }),
    }),
    filterList: build.mutation<FilterListApiResponse, FilterListApiArg>({
      query: (queryArg) => ({
        url: `/api/v1/Stock/FilterList`,
        method: 'POST',
        body: queryArg.filterListQuery,
      }),
    }),
  }),
  overrideExisting: false,
});
export { injectedRtkApi as generated };
export type AddNewApiResponse = /** status 200 Success */ number;
export type AddNewApiArg = {
  addNewStockCommand: AddNewStockCommand;
};
export type GetSingleApiResponse = /** status 200 Success */ GetSingleResponse;
export type GetSingleApiArg = {
  symbol: string;
};
export type GetAllApiResponse = /** status 200 Success */ GetAllResponse[];
export type GetAllApiArg = void;
export type FilterListApiResponse =
  /** status 200 Success */ FilterListResponse[];
export type FilterListApiArg = {
  filterListQuery: FilterListQuery;
};
export type AddNewStockCommand = {
  symbol?: string | null;
};
export type GetSingleResponse = {
  id?: number;
  symbol?: string | null;
  price?: number;
};
export type GetAllResponse = {
  id?: number;
  symbol?: string | null;
  price?: number;
};
export type FilterListResponse = {
  id?: number;
  symbol?: string | null;
};
export type FilterListQuery = {
  page?: number;
  take?: number;
  symbol?: string | null;
};
export const {
  useAddNewMutation,
  useGetSingleQuery,
  useGetAllQuery,
  useFilterListMutation,
} = injectedRtkApi;
