import React, { useEffect } from 'react';
import {
  useSearchParams,
  useNavigate,
  useLocation,
} from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { loginAsync } from './reducer';

function SocialLogin() {
  const [searchParams] = useSearchParams();
  const code = searchParams.get('code');
  const state = searchParams.get('state');

  const navigate = useNavigate();
  const location = useLocation();
  const from = location.state?.from?.pathname || '/';
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(loginAsync({ code, state }))
      .then(() => {
        console.log(from);
        navigate('/math');
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

export default SocialLogin;
