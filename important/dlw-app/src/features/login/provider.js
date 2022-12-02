export const githubAuthOptions = {
  baseUrl: 'https://github.com/login/oauth/authorize',
  access_type: 'offline',
  response_type: 'code',
  client_id: process.env.REACT_APP_GITHUB_CLIENT_ID,
  scope: ['read:user', 'user:email', 'read:repo_hook'],
  redirect_uri: '',
};

export const googleAuthOptions = {
  baseUrl: 'https://accounts.google.com/o/oauth2/v2/auth',
  access_type: 'offline',
  response_type: 'code',
  client_id: process.env.REACT_APP_GOOGLE_CLIENT_ID,
  scope: ['openid', 'email'],
  redirect_uri: process.env.REACT_APP_GOOGLE_REDIRECT_URL,
};

export const testAuthOptions = {
  baseUrl: 'https://localhost:8443/connect/authorize',
  access_type: 'offline',
  response_type: 'code',
  client_id: 'webclient',
  scope: ['openid', 'profile', 'HSS.IdentityServerAPI'],
  redirect_uri: 'http://localhost:3000/login/test',
  response_mode: 'query',
};

function getAuthorizeUrl(option, state) {
  const scope = option.scope.join('+');
  let url = `${option.baseUrl}?access_type=${option.access_type}&response_type=${option.response_type}&client_id=${option.client_id}&scope=${scope}&state=${state}`;

  if (option.redirect_uri) {
    const nonce = Date.now();
    url = `${url}&redirect_uri=${option.redirect_uri}&nonce=${nonce}`;
  }

  return url;
}

export default getAuthorizeUrl;
