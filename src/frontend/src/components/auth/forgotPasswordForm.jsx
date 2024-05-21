import React from "react";
import Form from "../common/form";
import auth from "../../services/authService";

class ForgotPasswordForm extends Form {
  state = {
    data: { email: "" },
    errors: {},
    propertyErrors: {
      email: ["EmailNotConfirmed", "EmailNotFound"],
    },
  };

  doRequest = async () => {
    auth
      .forgotPassword(this.state.data)
      .then((response) => {
        if (response.data && response.data.succeeded) {
          window.location = "/successForgotPassword";
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
        <h1>Сброс пароля</h1>
        <form onSubmit={this.handleSubmit}>
          {this.renderInput("email", "Почта")}
          {this.renderButton("Отправить")}
        </form>
      </div>
    );
  }
}

export default ForgotPasswordForm;
