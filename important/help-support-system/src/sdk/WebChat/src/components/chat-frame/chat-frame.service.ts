import HTTPService from "./http";

const ENDPOINT = process.env.ENDPOINT

export default class ChatFrameService extends HTTPService {
  constructor() {
    super();
  }

  async getService() {
    const response = await this._http.post(`${ENDPOINT}/api/auth/login/fake?tenantId=1`, null);
    return await response.json();
  }
}