import { generated } from './stockApiGenerated';

export const apiService = generated.enhanceEndpoints({
  addTagTypes: ['Stock'],
  endpoints: {
    filterStocks: {
      providesTags: ['Stock'],
      keepUnusedDataFor: 180000,
    },
    getStock: {
      providesTags: ['Stock'],
    },
    createStock: {
      invalidatesTags: ['Stock'],
    },
  },
});
