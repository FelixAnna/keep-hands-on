import React from 'react';
import {
  useNavigate,
} from 'react-router-dom';
import { useAuth } from './useAuth';

function AuthButton() {
  const history = useNavigate();
  const auth = useAuth();

  return auth.user ? (
    <p>
      Welcome!
      {' '}
      <button
        type="button"
        onClick={() => {
          auth.signout(() => history.push('/'));
        }}
      >
        Sign out
      </button>
    </p>
  ) : (
    <p>You are not logged in.</p>
  );
}

export default AuthButton;
