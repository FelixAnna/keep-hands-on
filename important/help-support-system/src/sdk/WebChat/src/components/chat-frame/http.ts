interface HTTPInterface {
  request: (url: string, data?: any, options?: RequestInit) => Promise<Response>;
  get: (url: string, options?: RequestInit) => Promise<Response>;
  post: (url: string, data: any, options?: RequestInit) => Promise<Response>;
}

const defaultHTTPParams: RequestInit = {
  method: 'GET', // *GET, POST, PUT, DELETE, etc.
  mode: 'cors', // no-cors, *cors, same-origin
  cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
  credentials: 'same-origin', // include, *same-origin, omit
  headers: {
    'Content-Type': 'application/json'
    // 'Content-Type': 'application/x-www-form-urlencoded',
  },
  redirect: 'follow', // manual, *follow, error
  referrerPolicy: 'no-referrer',
}

class HTTPUtil implements HTTPInterface {
  request(url: string, data?: any, options?: RequestInit) {
    const requestOptions = {...defaultHTTPParams, ...options};
    if (!!data) {
      requestOptions.body = data;
    }
    return fetch(url, requestOptions);
  }

  get(url: string, options?: RequestInit) {
    return this.request(url, undefined, {...options, method: 'GET'});
  }

  post(url: string, data: any, options?: RequestInit) {
    return this.request(url, data, {...options, method: 'POST'});
  }
}

export default abstract class HTTPService {
  protected _http: HTTPInterface;

  constructor() {
    this._http = new HTTPUtil();
  }
}
