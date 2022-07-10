import axios from 'axios';
const instance = axios.create({
    baseURL: 'http://192.168.1.12:8484/'
});

export function GetProblems(params){
    return instance.post(`/homework/math/multiple`, params)
}
