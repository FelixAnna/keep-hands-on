import React, { useEffect } from 'react';
import { useSelector } from 'react-redux';
import {
  useNavigate,
  useLocation,
} from 'react-router-dom';
import Link from '@mui/material/Link';
import getAuthorizeUrl, { githubAuthOptions, googleAuthOptions, testAuthOptions } from './provider';
import { currentLoginStatus } from './reducer';

function Login() {
  const [githubUrl, setGitHubUrl] = React.useState('#');
  const [googleUrl, setGoogleUrl] = React.useState('#');
  const [testUrl, setTestUrl] = React.useState('#');
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
      setTestUrl(getAuthorizeUrl(testAuthOptions, st));
    }
  }, [loginStatus]);

  return (
    <div>
      <div>Log In</div>
      <div>
        <Link href={githubUrl}>Login With Github</Link>
      </div>
      <div>
        <Link href={googleUrl}>Login With Google</Link>
      </div>
      <div>
        <Link href={testUrl}>Login With Test Authentication Server</Link>
      </div>
    </div>
  );
}

export default Login;
