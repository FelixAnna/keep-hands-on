export const githubAuthOptions = {
  baseUrl: 'https://github.com/login/oauth/authorize',
  access_type: 'offline',
  response_type: 'code',
  client_id: 'a4df124876d5001ef756',
  scope: ['read:user', 'user:email', 'read:repo_hook'],
  redirect_uri: '',
};

function getAuthorizeUrl(option, state) {
  const scope = option.scope.join('+');
  const url = `${option.baseUrl}?access_type=${option.access_type}&response_type=${option.response_type}&client_id=${option.client_id}&scope=${scope}&state=${state}`;
  return url;
}

export default getAuthorizeUrl;
