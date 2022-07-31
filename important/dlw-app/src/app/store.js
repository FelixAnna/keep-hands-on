import { configureStore } from '@reduxjs/toolkit';
import criteriaReducer from '../features/mathematicals/reducers/searchBar';
import authReducer from '../features/login/reducer';
import zdjReducer from '../features/zdj/reducer';

const store = configureStore({
  reducer: {
    criteria: criteriaReducer,
    auth: authReducer,
    zdj: zdjReducer,
  },
});

export default store;
