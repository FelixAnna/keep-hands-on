import React, { useState } from 'react';
import authContext from './useAuth';
 
export function ProvideAuth({ children }) {
    const auth = useProvideAuth();
    return (
      <authContext.Provider value={auth}>
        {children}
      </authContext.Provider>
    );
  }

function useProvideAuth() {
    const [user, setUser] = useState(null);

    const signin = cb => {
    return fakeAuth.signin(() => {
        setUser("user");
        cb();
    });
    };

    const signout = cb => {
    return fakeAuth.signout(() => {
        setUser(null);
        cb();
    });
    };

    return {
    user,
    signin,
    signout
    };
}