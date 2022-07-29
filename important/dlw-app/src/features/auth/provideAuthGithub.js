import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { useDispatch } from 'react-redux';
import authContext from './useAuth';
import { loginAsync, logout } from '../social/reducer';

function useProvideAuth() {
  const [user, setUser] = useState(null);
  const dispatch = useDispatch();

  const signin = (code, state, cb) => dispatch(loginAsync({ code, state }))
    .then(() => {
      setUser('user');
      cb();
    });

  const signout = (cb) => dispatch(logout()).then(() => {
    setUser(null);
    cb();
  });

  return {
    user,
    signin,
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
