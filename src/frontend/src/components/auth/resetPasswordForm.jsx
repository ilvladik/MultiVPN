import React from "react";
import Form from "../common/form";
import auth from "../../services/authService";

class ResetPasswordForm extends Form {
  state = {
    data: { password: "" },
    errors: {},
    propertyErrors: {
      password: [
        "PasswordRequiresDigit",
        "PasswordRequiresLower",
        "PasswordRequiresNonAlphanumeric",
        "PasswordRequiresUpper",
        "PasswordTooShort",
        "InvalidToken",
      ],
    },
  };

  doRequest = async () => {
    const { location } = this.props;
    const params = new URLSearchParams(location.search);
    auth
      .resetPassword(
        params.get("email") ?? "",
        params.get("code") ?? "",
        this.state.data.password
      )
      .then((response) => {
        if (response.data && response.data.succeeded) {
          window.location = "/login";
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
        <h1>Изменение пароля</h1>
        <form onSubmit={this.handleSubmit}>
          {this.renderInput("password", "Новый пароль", "password")}
          {this.renderButton("Отправить")}
        </form>
      </div>
    );
  }
}

export default ResetPasswordForm;
