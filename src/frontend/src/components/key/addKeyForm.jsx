import React from "react";
import Form from "../common/form";
import { createKey } from "../../services/keyService";
import { getOnlyUsedCountries } from "../../services/countryService";

class AddKeyForm extends Form {
  state = {
    data: { name: "", countryId: "" },
    errors: {},
    countries: [],
    propertyErrors: {
      name: ["OnlyOneKeyAllowed"],
      countryId: ["CountryNotFound"],
    },
  };

  componentDidMount() {
    getOnlyUsedCountries()
      .then((response) => {
        console.log(response.data);
        if (response.data && response.data.succeeded) {
          this.setState({ countries: response.data.value });
        }
      })
      .catch((error) => {
        if (error.response.status >= 500) window.location = "/internalError";
        const errors = this.validate(error.response.data);
        this.setState({ errors });
      });
  }
  doRequest = async () => {
    createKey({
      name: this.state.data.name,
      countryId: this.state.data.countryId,
    })
      .then((response) => {
        if (response.data && response.data.succeeded) {
          window.location = "/keys";
        }
      })
      .catch((error) => {
        console.log(error);
        if (error.response.status >= 500) window.location = "/internalError";
        const errors = this.validate(error.response.data);
        this.setState({ errors });
      });
  };

  render() {
    return (
      <div>
        <h1>Добавление ключа</h1>
        <form onSubmit={this.handleSubmit}>
          {this.renderInput("name", "Название")}
          {this.renderSelect("countryId", "Страна", this.state.countries)}
          {this.renderButton("Создать")}
        </form>
      </div>
    );
  }
}

export default AddKeyForm;
