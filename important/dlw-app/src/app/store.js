import { configureStore } from '@reduxjs/toolkit';
import criteriaReducer from '../features/mathematicals/reducers/searchBar';
import authReducer from '../features/login/reducer';

const store = configureStore({
  reducer: {
    criteria: criteriaReducer,
    auth: authReducer,
  },
});

export default store;
