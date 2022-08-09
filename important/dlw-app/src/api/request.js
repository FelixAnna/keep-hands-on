/* eslint-disable linebreak-style */
import axios from 'axios';

const instance = axios.create({
  baseURL: 'https://20.24.116.2/',
});

export function GetProblems(params) {
  return instance.post('/finance/homework/math/multiple', params);
}

export function SaveResults(data) {
  const token = localStorage.getItem('token');
  return instance.post('/finance/homework/math/save', data, { params: { access_code: token } });
}

export function LoginGithubUser(data) {
  return instance.get('/user/oauth2/github/login', { params: { code: data.code, state: data.state } });
}

export function SearchZdj(data) {
  return instance.post('/finance/zdj/search', data);
}

export function DeleteMemo(data) {
  const token = localStorage.getItem('token');
  return instance.delete(`/memo/memos/${data.id}`, { params: { access_code: token } });
}

export function AddMemo(data) {
  const token = localStorage.getItem('token');
  return instance.put('/memo/memos/', data, { params: { access_code: token } });
}

export function SearchMemo(data) {
  const token = localStorage.getItem('token');
  const start = data.StartDate.length === 10 ? data.StartDate.substring(5, 7) + data.StartDate.substring(8) : '0101';
  const end = data.EndDate.length === 10 ? data.EndDate.substring(5, 7) + data.EndDate.substring(8) : '1231';
  return instance.get('/memo/memos/recent', { params: { start, end, access_code: token } });
}
