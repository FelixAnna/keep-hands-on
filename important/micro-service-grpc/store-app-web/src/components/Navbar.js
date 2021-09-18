import React, { Component } from 'react'
import {Auth} from 'aws-amplify';

export default class Navbar extends Component {
  handleLogOut = async event => {
    event.preventDefault();
    try{
      Auth.signOut();
      this.props.auth.setAuthStatus(false);
      this.props.auth.setUser(null);
    } catch(error){
      console.log(error);
    }
  }
  render() {
    return (
      <nav className="navbar" role="navigation" aria-label="main navigation">
        <div className="navbar-brand">
          <a className="navbar-item" href="/">
            <img src="hexal-logo.png" width="112" height="28" alt="hexal logo" />
          </a>
        </div>

        <div id="navbarBasicExample" className="navbar-menu">
          <div className="navbar-start">
            <a href="/" className="navbar-item">
              Home
            </a>
            <a href="/products" className="navbar-item">
              Products
            </a>
            <a href="/admin" className="navbar-item">
              Admin
            </a>
          </div>

          <div className="navbar-end">
            <div className="navbar-item">
              {this.props.auth.isAuthenticated && this.props.auth.user &&
                <p>
                  Hello {this.props.auth.user.username}
                </p>
              }
              {this.props.auth.isAuthenticated &&
                 <div className="buttons">
                    <a href="/" className="button is-light" onClick={this.handleLogOut}>
                      Log Out
                    </a>
                 </div>
              }
              {!this.props.auth.isAuthenticated &&
                <div className="buttons">
                  <a href="/register" className="button is-primary">
                    <strong>Register</strong>
                  </a>
                  <a href="/login" className="button is-light">
                    Log in
                  </a>
                </div>
              }
            </div>
          </div>
        </div>
      </nav>
    )
  }
}
