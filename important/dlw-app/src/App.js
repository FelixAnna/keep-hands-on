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
import ProvideAuth from './features/auth/provideAuth';
import PrivateRoute from './features/auth/privateRoute';
import AuthButton from './features/auth/authButton';
import ResponsiveAppBar from './features/appbar/appbar';
import Home from './features/home';

function App() {
  return (
    <div className="App">
      <ResponsiveAppBar />
      <ProvideAuth>
        <BrowserRouter>
          <div>
            <AuthButton />
            <Routes>
              <Route path="home" element={<Home />} />
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
