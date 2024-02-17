import { configureStore } from '@reduxjs/toolkit'
import { stockApi } from './api/stockApiSlice';

export const store = configureStore({
    reducer: {
        [stockApi.reducerPath]: stockApi.reducer
    },
    middleware: (geDefaultMiddleware) => {
        return geDefaultMiddleware().concat(stockApi.middleware)
    }
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;