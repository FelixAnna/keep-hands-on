/* eslint-disable linebreak-style */
import axios from 'axios';

const financeInstance = axios.create({
  baseURL: 'http://localhost:8484/',
});

const memoIstance = axios.create({
  baseURL: 'http://localhost:8282/',
});

const userIstance = axios.create({
  baseURL: 'http://localhost:8181/',
});

export function GetProblems(params) {
  return financeInstance.post('/homework/math/multiple', params);
}

export function SaveResults(data) {
  const token = localStorage.getItem('token');
  return financeInstance.post('/homework/math/save', data, { params: { access_code: token } });
}

export function LoginGithubUser(data) {
  return userIstance.get('/oauth2/github/login', { params: { code: data.code, state: data.state } });
}

export function SearchZdj(data) {
  return financeInstance.post('/zdj/search', data);
}

export function SearchMemo(data) {
  const token = localStorage.getItem('token');
  const start = data.StartDate.length === 10 ? data.StartDate.substring(5, 7) + data.StartDate.substring(8) : '0101';
  const end = data.EndDate.length === 10 ? data.EndDate.substring(5, 7) + data.EndDate.substring(8) : '1231';
  return memoIstance.get('/memos/recent', { params: { start, end, access_code: token } });
}
