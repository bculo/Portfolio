import { createSlice } from '@reduxjs/toolkit'

interface CryptoState {
  cancelWindowVisible: boolean
}

const initialState: CryptoState = {
  cancelWindowVisible: false
}

export const cryptoSlice = createSlice({
  name: 'cryptoSlice',
  initialState,
  reducers: {
    showCancelWindow(state) {
      state.cancelWindowVisible = true;
    },
    hideCancelWindow(state) {
      state.cancelWindowVisible = false;
    }
  },
})

export const { showCancelWindow, hideCancelWindow } = cryptoSlice.actions
export default cryptoSlice.reducer