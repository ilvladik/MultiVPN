import React from "react";
import Form from "../common/form";
import { createCountry } from "../../services/countryService";

class AddCountryForm extends Form {
  state = {
    data: { name: "" },
    errors: {},
    propertyErrors: {
      name: ["CountryWithThisNameAlreadyExists"],
    },
  };

  doRequest = async () => {
    createCountry({ name: this.state.data.name })
      .then((response) => {
        if (response.data && response.data.succeeded) {
          window.location = "/countries";
        }
      })
      .catch((error) => {
        if (error.response.status >= 500) window.location = "/internalError";
        const errors = this.validate(error.response.data);
        this.setState({ errors });
      });
  };

  render() {
    return (
      <div>
        <h1>Добавление страны</h1>
        <form onSubmit={this.handleSubmit}>
          {this.renderInput("name", "Название")}
          {this.renderButton("Создать")}
        </form>
      </div>
    );
  }
}

export default AddCountryForm;
