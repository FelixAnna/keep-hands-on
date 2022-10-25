import React, { useEffect } from 'react';
import {
  useSearchParams,
  useNavigate,
} from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { loginAsync } from '../reducer';

function GoogleLogin() {
  const [searchParams] = useSearchParams();
  const code = searchParams.get('code');
  const state = searchParams.get('state');

  const navigate = useNavigate();
  const dispatch = useDispatch();
  useEffect(() => {
    if (state !== localStorage.getItem('state')) {
      return;
    }
    localStorage.removeItem('state');
    dispatch(loginAsync({ code, state, type: 'google' }))
      .then(() => {
        const from = sessionStorage.getItem('redirect_url');
        sessionStorage.removeItem('redirect_url');
        navigate(from);
      });
  }, []);

  return (
    <div>
      <div>Social Login Page: Log you in ...</div>
      <div>{code}</div>
      <div>{state}</div>
    </div>
  );
}

export default GoogleLogin;
