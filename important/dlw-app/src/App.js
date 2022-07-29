import React from 'react';
import {
  BrowserRouter,
  Routes,
  Route,
} from 'react-router-dom';
import './App.css';
import Mathematicals from './features/mathematicals/index';
import Login from './features/login';
import SocialLogin from './features/social/login';
import ProvideAuth from './features/auth/provideAuthGithub';
import PrivateRoute from './features/auth/privateRoute';
import ResponsiveAppBar from './features/appbar/appbar';
import Home from './features/home';
import Logout from './features/auth/logout';

function App() {
  return (
    <div className="App">
      <ProvideAuth>
        <ResponsiveAppBar />
        <BrowserRouter>
          <div>
            <Routes>
              <Route path="home" element={<Home />} />
              <Route path="logout" element={<Logout />} />
              <Route path="login" element={<Login />} />
              <Route path="login/social" element={<SocialLogin />} />
              <Route exact path="/math" element={<PrivateRoute><Mathematicals /></PrivateRoute>} />
            </Routes>
          </div>
        </BrowserRouter>
      </ProvideAuth>
    </div>
  );
}

export default App;
