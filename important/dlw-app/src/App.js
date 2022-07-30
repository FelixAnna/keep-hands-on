import React from 'react';
import {
  Routes,
  Route,
} from 'react-router-dom';
import './App.css';
import ResponsiveAppBar from './features/appbar/appbar';
import Home from './features/home';
import Logout from './features/login/oauth2/logout';
import Login from './features/login';
import GithubLogin from './features/login/oauth2/github';
import Mathematicals from './features/mathematicals/index';
import PrivateRoute from './features/auth/privateRoute';

function App() {
  return (
    <div className="App">
      <div>
        <ResponsiveAppBar />
        <Routes>
          <Route path="home" element={<Home />} />
          <Route path="logout" element={<Logout />} />
          <Route path="login" element={<Login />} />
          <Route path="login/github" element={<GithubLogin />} />
          <Route exact path="/math" element={<PrivateRoute><Mathematicals /></PrivateRoute>} />
        </Routes>
      </div>
    </div>
  );
}

export default App;
