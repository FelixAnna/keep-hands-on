import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useAuth } from './useAuth';
import { logout, currentLoginStatus } from '../social/reducer';

function Logout() {
  const history = useNavigate();
  const auth = useAuth();

  const loginStatus = useSelector(currentLoginStatus);

  const dispatch = useDispatch();
  React.useEffect(() => {
    if (!loginStatus) {
      return;
    }

    dispatch(logout());
    auth.signout(() => history.push('/'));
  }, []);

  return auth.user ? (
    <p>
      Log you out ...
    </p>
  ) : (
    <p>You are not logged in.</p>
  );
}

export default Logout;
