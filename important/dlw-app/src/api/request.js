import axios from 'axios';
const instance = axios.create({
    baseURL: 'http://localhost:8484/'
});

export function GetProblems(params){
    return instance.post(`/homework/math/multiple`, params)
}

export function SaveResults(data){
    return instance.post(`/homework/math/save`, data, {params: {access_code: "test"}})
}
