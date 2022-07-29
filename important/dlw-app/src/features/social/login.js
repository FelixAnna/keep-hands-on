import React, { useEffect } from 'react';
import { useSearchParams, Navigate } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import { loginAsync, currentLoginStatus } from './reducer';

function SocialLogin() {
  const [searchParams] = useSearchParams();
  const code = searchParams.get('code');
  const state = searchParams.get('state');
  const dispatch = useDispatch();

  const loginStatus = useSelector(currentLoginStatus);
  useEffect(() => {
    dispatch(loginAsync({ code, state }))
      .then(() => {
        console.log('welcome!');
      });
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
