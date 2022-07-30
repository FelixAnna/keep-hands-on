/* eslint-disable linebreak-style */
import axios from 'axios';

const instance = axios.create({
  baseURL: 'http://localhost:8484/',
});

const userIstance = axios.create({
  baseURL: 'http://localhost:8181/',
});

export function GetProblems(params) {
  return instance.post('/homework/math/multiple', params);
}

export function SaveResults(data) {
  const token = localStorage.getItem('token');
  return instance.post('/homework/math/save', data, { params: { access_code: token } });
}

export function LoginGithubUser(data) {
  return userIstance.get('/oauth2/github/login', { params: { code: data.code, state: data.state } });
}

export function SearchZdj(data) {
  return userIstance.get('/zdj/search', { params: { code: data.code, state: data.state } });
}
