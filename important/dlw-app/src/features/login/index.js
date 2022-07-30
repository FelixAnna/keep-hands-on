import React from 'react';
import { useLocation } from 'react-router-dom';
import Link from '@mui/material/Link';
import getAuthorizeUrl, { githubAuthOptions } from './provider';

function Login() {
  const st = 'state123';
  const url = getAuthorizeUrl(githubAuthOptions, st);

  const location = useLocation();
  const from = location.state?.from?.pathname || '/';
  console.log(from);
  sessionStorage.setItem('redirect_url', from);

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
