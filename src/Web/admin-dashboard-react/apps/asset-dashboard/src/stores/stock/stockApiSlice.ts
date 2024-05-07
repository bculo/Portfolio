import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { environment } from '../../environments/environment';
import { getAccessToken } from '../../utilities/token';

const authority = environment.oAuth2Config.authority;
const clientId = environment.oAuth2Config.client_id;

export const stockApi = createApi({
  reducerPath: 'stockApi',
  baseQuery: fetchBaseQuery({
    baseUrl: environment.stockApiBase,
    prepareHeaders: (headers) => {
      const access_token = getAccessToken(authority, clientId);
      headers.set('Authorization', `Bearer ${access_token}`);
      return headers;
    },
  }),
  endpoints: () => ({}),
});
