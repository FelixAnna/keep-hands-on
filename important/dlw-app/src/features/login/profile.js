import React from 'react';
import { useSelector } from 'react-redux';
import { currentUser } from './reducer';

function Profile() {
  const user = useSelector(currentUser);
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
