import { fork } from 'redux-saga/effects';
import { cryptoSaga } from './crypto/cryptoSaga';

export function* rootSaga() {
    yield fork(cryptoSaga);
}