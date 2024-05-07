import { fork, takeEvery, put, delay } from 'redux-saga/effects';
import { cryptoApiService } from './crypoApiService';
import { showCancelWindow, hideCancelWindow } from './cryptoSlice';

function* addItemWithDelay() {
    yield takeEvery([cryptoApiService.endpoints.addNewWithDelay.matchFulfilled], function* () {
        yield put(showCancelWindow())
        yield delay(10000);
        yield put(hideCancelWindow())
    });
}

export function* cryptoSaga() {
    yield fork(addItemWithDelay);
}