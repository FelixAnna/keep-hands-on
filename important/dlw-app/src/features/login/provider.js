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

function getAuthorizeUrl(option, state) {
  const scope = option.scope.join('+');
  const url = `${option.baseUrl}?access_type=${option.access_type}&response_type=${option.response_type}&client_id=${option.client_id}&scope=${scope}&state=${state}`;
  if (option.redirect_uri) {
    const nonce = Date.now();
    return `${url}&redirect_uri=${option.redirect_uri}&nonce=${nonce}`;
  }
  return url;
}

export default getAuthorizeUrl;
