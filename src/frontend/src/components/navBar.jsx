import React, { Component } from "react";
import { Link, NavLink } from "react-router-dom";
import auth, { isAuthenticated } from "../services/authService";

class NavBar extends Component {
  state = { isAuthenticated: false, isUser: false, isAdmin: false };

  async componentDidMount() {
    this.setState({ isAuthenticated: await auth.isAuthenticated() });
    this.setState({ isUser: await auth.hasRole("User") });
    this.setState({ isAdmin: await auth.hasRole("Admin") });
    console.log(this.state.isAuthenticated);
    console.log(this.state.isUser);
    console.log(this.state.isAdmin);
  }

  render() {
    // let isAuthenticated;
    // auth.isAuthenticated().then((value) => {
    //   isAuthenticated = value;
    // });

    // let isUser;
    // auth.hasRole("User").then((value) => {
    //   isUser = value;
    // });
    // let isAdmin;
    // auth.hasRole("Admin").then((value) => {
    //   isAdmin = value;
    // });
    return (
      <nav className="navbar navbar-expand-lg navbar-light bg-light">
        <Link className="navbar-brand" styles={{ marginLeft: 20 }} to="/">
          MultiVpn
        </Link>
        <button
          className="navbar-toggler"
          type="button"
          data-toggle="collapse"
          data-target="#navbarNavAltMarkup"
          aria-controls="navbarNavAltMarkup"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon" />
        </button>
        <div className="collapse navbar-collapse" id="navbarNavAltMarkup">
          <div className="navbar-nav">
            {this.state.isAdmin && (
              <React.Fragment>
                <NavLink className="nav-item nav-link" to="/countries">
                  Страны
                </NavLink>
                <NavLink className="nav-item nav-link" to="/keys">
                  Ключи
                </NavLink>
                <NavLink className="nav-item nav-link" to="/servers">
                  Сервера
                </NavLink>
              </React.Fragment>
            )}
            {this.state.isUser && (
              <React.Fragment>
                <NavLink className="nav-item nav-link" to="/keys">
                  Ключи
                </NavLink>
              </React.Fragment>
            )}
            {!this.state.isAuthenticated && (
              <React.Fragment>
                <NavLink className="nav-item nav-link" to="/login">
                  Login
                </NavLink>
                <NavLink className="nav-item nav-link" to="/register">
                  Register
                </NavLink>
              </React.Fragment>
            )}
            {this.state.isAuthenticated && (
              <React.Fragment>
                <NavLink className="nav-item nav-link" to="/logout">
                  Logout
                </NavLink>
              </React.Fragment>
            )}
          </div>
        </div>
      </nav>
    );
  }
}

export default NavBar;
