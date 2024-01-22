import { createSlice } from '@reduxjs/toolkit';

interface CounterState {
    value: number;
}

const initialState: CounterState = {
    value: 0
};

const counterSlice = createSlice({
    name: 'counter',
    initialState,
    reducers: {
        increment(state) {
            state.value++;
        },
        decrement(state) {
            state.value--;
        },
        reset(state) {
            state.value = 0;
        }
    }
})

export const { increment, decrement } = counterSlice.actions;
export default counterSlice.reducer;