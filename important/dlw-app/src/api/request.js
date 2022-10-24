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

export function LoginGithubUser(data) {
  return instance.get('/user/oauth2/github/login', { params: { code: data.code } });
}

export function SearchZdj(data) {
  return instance.post('/finance/zdj/search', data);
}

export function DeleteMemo(data) {
  return instance.delete(`/memo/memos/${data.id}`);
}

export function AddMemo(data) {
  return instance.put('/memo/memos/', data);
}

export function SearchMemo(data) {
  const start = data.StartDate.length === 10 ? data.StartDate.substring(5, 7) + data.StartDate.substring(8) : '0101';
  const end = data.EndDate.length === 10 ? data.EndDate.substring(5, 7) + data.EndDate.substring(8) : '1231';
  return instance.get('/memo/memos/recent', { params: { start, end } });
}

export function GetLunarDate(data) {
  return instance.get('/date/date/lunar', { params: { date: data.date } });
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
