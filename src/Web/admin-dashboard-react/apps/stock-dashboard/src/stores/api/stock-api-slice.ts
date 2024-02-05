import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { oAuth2Config } from '../../configs/oauth-config';

export const stockApi = createApi({
    reducerPath: 'api',
    baseQuery: fetchBaseQuery({
        baseUrl: 'http://localhost:32034',
        prepareHeaders: (headers) => {
            const storageItemString = localStorage.getItem(`oidc.user:${oAuth2Config.authority}:${oAuth2Config.client_id}`)
            if(!storageItemString) {
                return headers;
            }
            const token = JSON.parse(storageItemString!).access_token;
            headers.set("Authorization", `Bearer: ${token}`)
            return headers;
        }
    }),
    endpoints: () => ({}),
});