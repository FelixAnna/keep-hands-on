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
import GoogleLogin from './features/login/oauth2/google';
import TestLogin from './features/login/oauth2/test';
import Mathematicals from './features/mathematicals/index';
import PrivateRoute from './features/auth/privateRoute';
import ZdjSearch from './features/zdj';
import Zhuyin from './features/zhuyin'
import MemoSearch from './features/memo';
import Profile from './features/login/profile';

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
          <Route path="login/google" element={<GoogleLogin />} />
          <Route path="login/test" element={<TestLogin />} />
          <Route path="zhuyin" element={<Zhuyin />} />
          <Route path="zdj" element={<ZdjSearch />} />
          <Route exact path="/profile" element={<PrivateRoute><Profile /></PrivateRoute>} />
          <Route exact path="/memo" element={<PrivateRoute><MemoSearch /></PrivateRoute>} />
          <Route exact path="/math" element={<PrivateRoute><Mathematicals /></PrivateRoute>} />
        </Routes>
      </div>
    </div>
  );
}

export default App;
