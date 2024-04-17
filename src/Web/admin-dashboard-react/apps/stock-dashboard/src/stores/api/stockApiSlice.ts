import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { environment } from '../../environments/environment';

const authority = environment.oAuth2Config.authority;
const clientId = environment.oAuth2Config.client_id;

export const stockApi = createApi({
    reducerPath: 'api',
    baseQuery: fetchBaseQuery({
        baseUrl: environment.stockApiBase,
        prepareHeaders: (headers) => {
            const storageItemString = localStorage.getItem(`oidc.user:${authority}:${clientId}`)
            if(!storageItemString) {
                return headers;
            }
            const token = JSON.parse(storageItemString!).access_token;
            headers.set("Authorization", `Bearer ${token}`)
            return headers;
        }
    }),
    endpoints: () => ({}),
});