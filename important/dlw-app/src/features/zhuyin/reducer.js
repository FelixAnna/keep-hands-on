import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import pinyin from 'pinyin';

// Distrct/Street/Community/MinPrice/MaxPrice/Version/SortKey/Page/Size
const initialState = {
  Criteria: {
    Keywords: '',
  },
  Pinyin: [],
};

// The function below is called a thunk and allows us to perform async logic. It
// can be dispatched like a regular action: `dispatch(incrementAsync(10))`. This
// will call the thunk with the `dispatch` function as the first argument. Async
// code can then be executed and other actions can be dispatched. Thunks are
// typically used to make async requests.
export const loadAsync = createAsyncThunk(
  'zhuyin/Search',
  async (criteria) => {
    const response = pinyin(criteria.Keywords, {
      heteronym: false,
      segment: true,
      group: true,
    });
    // The value we return becomes the `fulfilled` action payload
    console.log(response);
    return response;
  },
);

export const zhuyinSlice = createSlice({
  name: 'zhuyin',
  initialState,
  // The `reducers` field lets us define reducers and generate associated actions
  reducers: {
    saveCriteria(state, action) {
      state.Pinyin = [];
      state.Criteria = action.payload;
    },
    clearAll(state) {
      if (JSON.stringify(state.Criteria) === JSON.stringify(initialState.Criteria)) {
        return;
      }
      state.Criteria = initialState.Criteria;
      state.Pinyin = [];
    },
  },
  // The `extraReducers` field lets the slice handle actions defined elsewhere,
  // including actions generated by createAsyncThunk or in other slices.
  extraReducers: (builder) => {
    builder
      .addCase(loadAsync.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(loadAsync.fulfilled, (state, action) => {
        const items = action.payload;
        state.status = 'idle';
        console.log(action.payload);
        state.Pinyin = items;
      });
  },
});

export const { saveCriteria, clearAll } = zhuyinSlice.actions;
export const currentCriteria = (state) => state.zhuyin.Criteria;
export const currentItems = (state) => state.zhuyin.Pinyin;

export default zhuyinSlice.reducer;
