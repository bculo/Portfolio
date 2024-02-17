import { generated } from "./generated";

export const apiService = generated
    .enhanceEndpoints({
        addTagTypes: ['Stock'],
        endpoints: {
            filterStocks: {
                providesTags: ['Stock']
            },
            getStock: {
                providesTags: ['Stock']
            },
            createStock: {
                invalidatesTags: ['Stock']
            }
        }
    })