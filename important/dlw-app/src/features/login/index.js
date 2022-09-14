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
  const [url, setUrl] = React.useState('#');
  const location = useLocation();
  const from = location.state?.from?.pathname || '/';
  sessionStorage.setItem('redirect_url', from);

  const navigate = useNavigate();
  const loginStatus = useSelector(currentLoginStatus);
  const makeid = (length) => {
    let result = '';
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    const charactersLength = characters.length;
    for (let i = 0; i < length; i += 1) {
      result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
  };

  useEffect(() => {
    if (loginStatus) {
      navigate(from);
    } else {
      const st = makeid(20);
      localStorage.setItem('state', st);
      setUrl(getAuthorizeUrl(githubAuthOptions, st));
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
