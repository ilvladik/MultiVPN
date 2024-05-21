import React from "react";
import Form from "../common/form";
import auth from "../../services/authService";
import { NavLink } from "react-router-dom";

class RegisterForm extends Form {
  state = {
    data: { email: "", password: "" },
    errors: {},
    propertyErrors: {
      email: ["InvalidEmail", "DuplicateEmail"],
      password: [
        "PasswordRequiresDigit",
        "PasswordRequiresLower",
        "PasswordRequiresNonAlphanumeric",
        "PasswordRequiresUpper",
        "PasswordTooShort",
      ],
    },
  };

  doRequest = async () => {
    auth
      .register(this.state.data)
      .then((response) => {
        if (response.data && response.data.succeeded) {
          window.location = "/successRegistration";
        } else {
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
        <h1>Регистрация</h1>
        <form onSubmit={this.handleSubmit}>
          {this.renderInput("email", "Почта")}
          {this.renderInput("password", "Пароль", "password")}
          <NavLink to="/login">Уже зарегистрированы?</NavLink>
          {this.renderButton("Зарегистрироваться")}
        </form>
      </div>
    );
  }
}

export default RegisterForm;
