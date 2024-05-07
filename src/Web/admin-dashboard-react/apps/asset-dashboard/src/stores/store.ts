import { cryptoSlice } from './crypto/cryptoSlice';
import { Middleware, configureStore } from '@reduxjs/toolkit';
import { stockApiService } from './stock/stockApiService';
import { cryptoApiService } from './crypto/crypoApiService';
import createSagaMiddleware from 'redux-saga'
import { rootSaga } from './rootSaga';

const sagaMiddleware = createSagaMiddleware()
const middleware: Middleware[] = [sagaMiddleware, stockApiService.middleware, cryptoApiService.middleware]

export const store = configureStore({
  reducer: {
    [cryptoSlice.reducerPath]: cryptoSlice.reducer,
    [stockApiService.reducerPath]: stockApiService.reducer,
    [cryptoApiService.reducerPath]: cryptoApiService.reducer,
  },
  middleware: (geDefaultMiddleware) => {
    return geDefaultMiddleware().concat(middleware)
  },
});

sagaMiddleware.run(rootSaga)

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
