import React, { useState } from 'react';
import PropTypes from 'prop-types';
import authContext from './useAuth';

const fakeAuth = {
  isAuthenticated: false,
  signin(callback) {
    fakeAuth.isAuthenticated = true;
    setTimeout(callback, 100); // fake async
  },
  signout(callback) {
    fakeAuth.isAuthenticated = false;
    setTimeout(callback, 100);
  },
};

function useProvideAuth() {
  const [user, setUser] = useState(null);

  const signin = (cb) => fakeAuth.signin(() => {
    setUser('user');
    cb();
  });

  const signout = (cb) => fakeAuth.signout(() => {
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
