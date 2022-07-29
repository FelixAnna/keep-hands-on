import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import jwt from 'jwt-decode';
import { LoginGithubUser } from '../../api/request';

const initialState = {
  Status: 'initial',
  isAuthenticated: false,
  User: {},
};

// The function below is called a thunk and allows us to perform async logic. It
// can be dispatched like a regular action: `dispatch(incrementAsync(10))`. This
// will call the thunk with the `dispatch` function as the first argument. Async
// code can then be executed and other actions can be dispatched. Thunks are
// typically used to make async requests.
export const loginAsync = createAsyncThunk(
  'social/Login',
  async (params) => {
    const response = await LoginGithubUser(params);
    // The value we return becomes the `fulfilled` action payload
    return response.data;
  },
);
export const socialSlice = createSlice({
  name: 'social',
  initialState,
  // The `reducers` field lets us define reducers and generate associated actions
  reducers: {
    logout(state) {
      state.isAuthenticated = false;
      state.User = {};
    },
  },
  // The `extraReducers` field lets the slice handle actions defined elsewhere,
  // including actions generated by createAsyncThunk or in other slices.
  extraReducers: (builder) => {
    builder
      .addCase(loginAsync.pending, (state) => {
        state.Status = 'loading';
      })
      .addCase(loginAsync.fulfilled, (state, action) => {
        state.Status = 'idle';
        const { token } = action.payload;
        const decoded = jwt(token);
        state.User = { Email: decoded.email, UserId: decoded.userId };
        state.isAuthenticated = true;
        localStorage.setItem('token', token);
      });
  },
});

export const { logout } = socialSlice.actions;
export const currentLoginStatus = (state) => state.social.isAuthenticated;
export const currentUser = (state) => state.social.User;
export const currentStatus = (state) => state.social.Status;

export default socialSlice.reducer;
