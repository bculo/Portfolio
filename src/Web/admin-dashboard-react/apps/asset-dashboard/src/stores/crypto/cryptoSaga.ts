import { fork, takeEvery, put, delay } from 'redux-saga/effects';
import { cryptoApiService } from './crypoApiService';
import { showCancelToast, hideCancelToast } from './cryptoSlice';

function* addItemWithDelay() {
    yield takeEvery([cryptoApiService.endpoints.addNewWithDelay.matchFulfilled], function* (action) {
        yield put(showCancelToast(action.payload))
        yield delay(10000);
        yield put(hideCancelToast())
    });
}

export function* cryptoSaga() {
    yield fork(addItemWithDelay);
}