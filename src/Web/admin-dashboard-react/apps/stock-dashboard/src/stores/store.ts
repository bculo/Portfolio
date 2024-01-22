import { configureStore } from '@reduxjs/toolkit'
import counterReducer from './counter/counter-slice'
import { stockApi } from './api/stock-api-slice';

export const store = configureStore({
    reducer: {
        counter: counterReducer,
        [stockApi.reducerPath]: stockApi.reducer
    },
    middleware: (geDefaultMiddleware) => {
        return geDefaultMiddleware().concat(stockApi.middleware)
    }
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;