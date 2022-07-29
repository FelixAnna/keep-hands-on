import React, { useEffect } from 'react';
import { useSearchParams, Navigate } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { currentLoginStatus } from './reducer';
import { useAuth } from '../auth/useAuth';

function SocialLogin() {
  const [searchParams] = useSearchParams();
  const code = searchParams.get('code');
  const state = searchParams.get('state');

  const loginStatus = useSelector(currentLoginStatus);
  const auth = useAuth();

  useEffect(() => {
    auth.signin(code, state);
  }, []);

  return (
    <div>
      <div>Social Login Page: Log you in ...</div>
      <div>{code}</div>
      <div>{state}</div>
      <div>
        { loginStatus ? <Navigate to="/math" replace="true" /> : '' }
      </div>
    </div>
  );
}

export default SocialLogin;
