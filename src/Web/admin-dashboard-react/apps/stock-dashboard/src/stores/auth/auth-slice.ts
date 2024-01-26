import { PayloadAction, createSlice } from '@reduxjs/toolkit';

interface AuthState {
    access_token: string | null;
}

const initialState: AuthState = {
    access_token: null
};

const authSlice = createSlice({
    name: 'counter',
    initialState,
    reducers: {
        setToken(state, action: PayloadAction<string>) {
            state.access_token = action.payload
        },
    }
})

export const { setToken } = authSlice.actions;
export default authSlice.reducer;