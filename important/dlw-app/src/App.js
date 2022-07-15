import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

import './App.css';
import Mathematicals from './features/mathematicals/index';
import Login from './features/login';

function App() {
  return (
    <div className="App">
      <BrowserRouter>
        <Routes>
          <Route path="login" element={<Login />} />
          <Route path="math" element={<Mathematicals />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
