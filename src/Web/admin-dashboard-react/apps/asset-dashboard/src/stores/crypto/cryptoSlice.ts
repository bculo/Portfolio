import { PayloadAction, createSlice } from '@reduxjs/toolkit'
import { cryptoApiService } from './crypoApiService';

interface CryptoState {
  cancelToastVisible: boolean
  temporaryId: string | null;
}

const initialState: CryptoState = {
  cancelToastVisible: false,
  temporaryId: null
}

export const cryptoSlice = createSlice({
  name: 'cryptoSlice',
  initialState,
  reducers: {
    showCancelToast(state, action: PayloadAction<string>) {
      state.cancelToastVisible = true;
      state.temporaryId = action.payload;
    },
    hideCancelToast(state) {
      state.cancelToastVisible = false;
      state.temporaryId = null;
    }
  },
  extraReducers: (builder) => {
    builder.addMatcher(cryptoApiService.endpoints.undoAddNewDelay.matchPending, (state, _) => {
      state.cancelToastVisible = false;
      state.temporaryId = null;
    })
  }
})

export const { showCancelToast, hideCancelToast } = cryptoSlice.actions
export default cryptoSlice.reducer