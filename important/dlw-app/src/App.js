import React from 'react';
import {
  BrowserRouter,
  Routes,
  Route,
  Link,
} from 'react-router-dom';

import './App.css';
import Mathematicals from './features/mathematicals/index';
import Login from './features/login';
import SocialLogin from './features/social/login';
import { ProvideAuth } from './features/social/provideAuth';
import PrivateRoute from './features/social/privateRoute';
import AuthButton from './features/social/authButton';

function App() {
  return (
    <div className="App">
      <ProvideAuth>
        <BrowserRouter>
          <div>
            <AuthButton />
            <ul>
              <li>
                <Link to="/logi/social">Public Page</Link>
              </li>
              <li>
                <Link to="/math">Protected Page</Link>
              </li>
            </ul>

            <Routes>
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
