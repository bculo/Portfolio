import { configureStore } from '@reduxjs/toolkit';
import { stockApiService } from './api/stockApiService';
import { cryptoApiService } from './api/crypoApiService';

export const store = configureStore({
  reducer: {
    [stockApiService.reducerPath]: stockApiService.reducer,
    [cryptoApiService.reducerPath]: cryptoApiService.reducer,
  },
  middleware: (geDefaultMiddleware) => {
    return geDefaultMiddleware()
      .concat(stockApiService.middleware)
      .concat(cryptoApiService.middleware);
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
