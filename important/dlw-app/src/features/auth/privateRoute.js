import React from 'react';
import {
  Navigate,
  useLocation,
} from 'react-router-dom';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import { currentLoginStatus } from '../login/reducer';

// A wrapper for <Route> that redirects to the login
// screen if you're not yet authenticated.
function PrivateRoute({ children }) {
  const location = useLocation();
  const loginStatus = useSelector(currentLoginStatus);

  if (!loginStatus) {
    return <Navigate to="/login" state={{ from: location }} />;
  }

  return children;
}

PrivateRoute.propTypes = {
  children: PropTypes.node.isRequired,
};

export default PrivateRoute;
