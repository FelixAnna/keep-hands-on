import HTTPService from "./http";

const ENDPOINT = process.env.ENDPOINT

export default class ChatFrameService extends HTTPService {
  constructor() {
    super();
  }

  async getService() {
    const response = await this._http.get(`${ENDPOINT}/register`);
    return await response.json();
  }
}