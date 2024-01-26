import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { RootState } from '../store';

export const stockApi = createApi({
    reducerPath: 'api',
    baseQuery: fetchBaseQuery({
        baseUrl: 'http://localhost:32034',
        prepareHeaders: (headers, { getState }) => {
            console.log('prepareHeaders is called');
            const token = (getState() as RootState).auth.access_token; // we are now consuming token from new created authSlice
            console.log(token)
            if (token) {
              headers.set('Authorization', `Bearer ${token}`);
            }
            return headers;
        }
    }),
    endpoints: () => ({}),
});