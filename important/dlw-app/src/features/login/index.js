import React, { useEffect } from 'react';
import { useSelector } from 'react-redux';
import {
  useNavigate,
  useLocation,
} from 'react-router-dom';
import Link from '@mui/material/Link';
import getAuthorizeUrl, { githubAuthOptions } from './provider';
import { currentLoginStatus } from './reducer';

function Login() {
  const st = 'state123';
  const url = getAuthorizeUrl(githubAuthOptions, st);

  const location = useLocation();
  const from = location.state?.from?.pathname || '/';
  sessionStorage.setItem('redirect_url', from);

  const navigate = useNavigate();
  const loginStatus = useSelector(currentLoginStatus);
  useEffect(() => {
    if (loginStatus) {
      navigate(from);
    }
  }, [loginStatus]);

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
