import React, { useEffect } from 'react';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { currentLoginStatus, currentUser } from './reducer';

function Profile() {
  const navigate = useNavigate();
  const loginStatus = useSelector(currentLoginStatus);
  const user = useSelector(currentUser);
  useEffect(() => {
    if (!loginStatus) {
      navigate('/login');
    }
  }, []);

  return (
    <div>
      <div>Profile Page</div>
      <div>
        Welcome,
        {user.Email}
        !
      </div>
    </div>
  );
}

export default Profile;
