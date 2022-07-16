import { configureStore } from '@reduxjs/toolkit';
import criteriaReducer from '../features/mathematicals/reducers/searchBar';
import socialReducer from '../features/social/reducer';

const store = configureStore({
  reducer: {
    criteria: criteriaReducer,
    social: socialReducer,
  },
});

export default store;
