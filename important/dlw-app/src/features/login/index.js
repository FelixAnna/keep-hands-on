import React from 'react';
import Link from '@mui/material/Link';
import getAuthorizeUrl, { githubAuthOptions } from './provider';

function Login() {
  const st = 'state123';
  const url = getAuthorizeUrl(githubAuthOptions, st);

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
