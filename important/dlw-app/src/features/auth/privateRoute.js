import React from 'react';
import {
  Navigate,
  useLocation,
} from 'react-router-dom';
import PropTypes from 'prop-types';
import { useAuth } from './useAuth';

// A wrapper for <Route> that redirects to the login
// screen if you're not yet authenticated.
function PrivateRoute({ children }) {
  const auth = useAuth();
  const location = useLocation();

  if (!auth.user) {
    return <Navigate to="/login" state={{ from: location }} />;
  }

  return children;
}

PrivateRoute.propTypes = {
  children: PropTypes.node.isRequired,
};

export default PrivateRoute;
