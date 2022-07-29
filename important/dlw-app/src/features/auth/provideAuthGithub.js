import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { useDispatch } from 'react-redux';
import authContext from './useAuth';
import { loginAsync, logout } from '../social/reducer';

function useProvideAuth() {
  const [user, setUser] = useState(null);
  const dispatch = useDispatch();

  const signinWithGithub = (code, state, callback) => dispatch(loginAsync({ code, state }))
    .then(() => {
      setUser('user');
      callback();
    });

  const signout = (callback) => dispatch(logout()).then(() => {
    setUser(null);
    callback();
  });

  return {
    user,
    signinWithGithub,
    signout,
  };
}

function ProvideAuth({ children }) {
  const auth = useProvideAuth();
  return (
    <authContext.Provider value={auth}>
      {children}
    </authContext.Provider>
  );
}

ProvideAuth.propTypes = {
  children: PropTypes.node.isRequired,
};

export default ProvideAuth;
