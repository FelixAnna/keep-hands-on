import React, { useEffect } from 'react';
import { useSelector } from 'react-redux';
import {
  useNavigate,
  useLocation,
} from 'react-router-dom';
import Link from '@mui/material/Link';
import getAuthorizeUrl, { githubAuthOptions, googleAuthOptions } from './provider';
import { currentLoginStatus } from './reducer';

function Login() {
  const [githubUrl, setGitHubUrl] = React.useState('#');
  const [googleUrl, setGoogleUrl] = React.useState('#');
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
      setGitHubUrl(getAuthorizeUrl(githubAuthOptions, st));
      setGoogleUrl(getAuthorizeUrl(googleAuthOptions, st));
    }
  }, [loginStatus]);

  return (
    <div>
      <div>Sign In</div>
      <div>
        <Link href={githubUrl}>Sign in With Github</Link>
      </div>
      <div>
        <Link href={googleUrl}>Sign in With Google</Link>
      </div>
    </div>
  );
}

export default Login;
