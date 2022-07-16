import React from 'react';
import Link from '@mui/material/Link';
import getAuthorizeUrl, { githubAuthOptions } from './provider';

function Login() {
  const state = 'state123';
  const url = getAuthorizeUrl(githubAuthOptions, state);
  return (
    <div>
      <div>Login Page</div>
      <div>
        <Link href={url}>Login With Github</Link>
      </div>
    </div>
  );
}

export default Login;
