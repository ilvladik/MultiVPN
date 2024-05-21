import React from "react";
import Form from "../common/form";
import { createServer } from "../../services/serverService";
import { getCountries } from "../../services/countryService";

class AddServerForm extends Form {
  state = {
    data: { name: "", apiUrl: "", countryId: "" },
    errors: {},
    countries: [],
    propertyErrors: {
      apiUrl: ["ServerWithThisAddressAlreadyExists", "InvalidServerAddress"],
      countryId: ["CountryNotFound"],
    },
  };

  componentDidMount() {
    console.log("ok");
    getCountries()
      .then((response) => {
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
    createServer({
      name: this.state.data.name,
      apiUrl: this.state.data.apiUrl,
      countryId: this.state.data.countryId,
    })
      .then((response) => {
        if (response.data && response.data.succeeded) {
          window.location = "/servers";
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
        <h1>Добавление сервера</h1>
        <form onSubmit={this.handleSubmit}>
          {this.renderInput("name", "Название")}
          {this.renderInput("apiUrl", "Api адрес")}
          {this.renderSelect("countryId", "Страна", this.state.countries)}
          {this.renderButton("Создать")}
        </form>
      </div>
    );
  }
}

export default AddServerForm;
