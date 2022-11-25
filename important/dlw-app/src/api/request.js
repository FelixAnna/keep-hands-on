/* eslint-disable linebreak-style */
import axios from 'axios';

const instance = axios.create({
  baseURL: process.env.REACT_APP_API_URL,
});

export function GetProblems(params) {
  return instance.post('/finance/homework/math/multiple', params);
}

export function SaveResults(data) {
  return instance.post('/finance/homework/math/save', data);
}

export function LoginGithubUser({ code }) {
  return instance.get('/user/oauth2/github/login', { params: { code } });
}

export function LoginGoogleUser({ code }) {
  return instance.get('/user/oauth2/google/login', { params: { code } });
}

export function SearchZdj(data) {
  return instance.post('/finance/zdj/search', data);
}

export function DeleteMemo({ id }) {
  return instance.delete(`/memo/memos/${id}`);
}

export function AddMemo(data) {
  return instance.put('/memo/memos/', data);
}

export function SearchMemo({ StartDate, EndDate }) {
  const start = StartDate.length === 10 ? StartDate.substring(5, 7) + StartDate.substring(8) : '0101';
  const end = EndDate.length === 10 ? EndDate.substring(5, 7) + EndDate.substring(8) : '1231';
  return instance.get('/memo/memos/recent', { params: { start, end } });
}

export function GetLunarDate({ date }) {
  return instance.get('/date/date/lunar', { params: { date } });
}

// Request interceptors for API calls
instance.interceptors.request.use(
  (config) => {
    // eslint-disable-next-line no-param-reassign
    config.headers.Authorization = `Bearer ${localStorage.getItem('token')}`;
    return config;
  },
  (error) => Promise.reject(error),
);
