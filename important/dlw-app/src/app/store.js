import { configureStore } from '@reduxjs/toolkit';
import criteriaReducer from '../features/mathematicals/reducers/searchBar';

const store = configureStore({
  reducer: {
    criteria: criteriaReducer,
  },
});

export default store;
