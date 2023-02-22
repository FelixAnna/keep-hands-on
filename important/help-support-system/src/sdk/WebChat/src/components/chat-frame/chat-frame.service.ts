import HTTPService from "./http";

const ENDPOINT = process.env.ENDPOINT

export default class ChatFrameService extends HTTPService {
  constructor() {
    super();
  }

  async getService () {
    const tenantId = sessionStorage.getItem("TENANT_ID") ?? 1;
    const response = await this._http.post(`${ENDPOINT}/api/auth/login/fake?tenantId=${tenantId}`, null);
    return await response.json();
  }
}