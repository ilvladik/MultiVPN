import React from "react";
import Form from "../common/form";
import auth from "../../services/authService";
import { NavLink } from "react-router-dom";

class LoginForm extends Form {
  state = {
    data: { email: "", password: "" },
    errors: {},
    propertyErrors: {
      email: ["EmailNotConfirmed", "EmailNotFound", "InvalidEmailOrPassword"],
      password: ["SignInIsLockedOut"],
    },
  };

  doRequest = async () => {
    auth
      .login(this.state.data)
      .then((response) => {
        if (response.data && response.data.succeeded) {
          window.location = "/";
        }
      })
      .catch((error) => {
        if (error.response.status >= 500) window.location = "/500";
        const errors = this.validate(error.response.data);
        this.setState({ errors });
      });
  };

  render() {
    return (
      <div>
        <h1>Вход</h1>
        <form onSubmit={this.handleSubmit}>
          {this.renderInput("email", "Почта")}
          {this.renderInput("password", "Пароль", "password")}
          <NavLink to="/forgotPassword">Забыли пароль?</NavLink>
          {this.renderButton("Войти")}
        </form>
      </div>
    );
  }
}

export default LoginForm;
