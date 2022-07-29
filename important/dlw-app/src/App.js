import React from 'react';
import {
  Routes,
  Route,
} from 'react-router-dom';
import './App.css';
import ResponsiveAppBar from './features/appbar/appbar';
import Home from './features/home';
import Logout from './features/auth/logout';
import Login from './features/login';
import SocialLogin from './features/social/github';
import Mathematicals from './features/mathematicals/index';
import PrivateRoute from './features/auth/privateRoute';

function App() {
  return (
    <div className="App">
      <div>
        <ResponsiveAppBar />
        <h1>Auth Example</h1>
        <Routes>
          <Route path="home" element={<Home />} />
          <Route path="logout" element={<Logout />} />
          <Route path="login" element={<Login />} />
          <Route path="login/social" element={<SocialLogin />} />
          <Route exact path="/math" element={<PrivateRoute><Mathematicals /></PrivateRoute>} />
        </Routes>
      </div>
    </div>
  );
}

export default App;
