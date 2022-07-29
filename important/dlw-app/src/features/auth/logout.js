import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { logout, currentLoginStatus } from '../social/reducer';

function Logout() {
  const history = useNavigate();
  const loginStatus = useSelector(currentLoginStatus);

  const dispatch = useDispatch();
  React.useEffect(() => {
    if (!loginStatus) {
      return;
    }

    dispatch(logout()).then(() => {
      history.push('/');
    });
  }, []);

  return loginStatus ? (
    <p>
      Log you out ...
    </p>
  ) : (
    <p>You are not logged in.</p>
  );
}

export default Logout;
