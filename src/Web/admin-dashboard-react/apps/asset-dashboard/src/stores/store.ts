import { configureStore } from '@reduxjs/toolkit';
import { stockApi } from './api/stockApiSlice';
import { apiService } from './api/apiService';
import { helloWorld } from '@admin-dashboard-react/api'

console.log(helloWorld)

export const store = configureStore({
  reducer: {
    [stockApi.reducerPath]: apiService.reducer,
  },
  middleware: (geDefaultMiddleware) => {
    return geDefaultMiddleware().concat(apiService.middleware);
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
