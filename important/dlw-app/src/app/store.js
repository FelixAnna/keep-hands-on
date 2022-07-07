import { configureStore } from '@reduxjs/toolkit';
import counterReducer from '../features/counter/counterSlice';
import criteriaReducer from '../features/mathematicals/reducers/searchBar';


export const store = configureStore({
  reducer: {
    counter: counterReducer,
    criteria: criteriaReducer
  },
});
