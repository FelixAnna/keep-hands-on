import React, { useEffect } from 'react';
import {
  useSearchParams,
  useNavigate,
} from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { loginAsync } from '../reducer';

function GithubLogin() {
  const [searchParams] = useSearchParams();
  const code = searchParams.get('code');
  const state = searchParams.get('state');

  const navigate = useNavigate();
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(loginAsync({ code, state }))
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

export default GithubLogin;
