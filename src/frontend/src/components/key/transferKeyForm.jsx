import React from "react";
import Form from "../common/form";
import { withRouter } from "react-router-dom/cjs/react-router-dom.min";
import { getServers } from "../../services/serverService";
import { getKey, transferKeyToNewServer } from "../../services/keyService";

class TransferKeyForm extends Form {
  state = {
    data: { serverId: "", name: "", country: "", serverAddress: "" },
    errors: {},
    servers: [],
    propertyErrors: {
      serverId: ["CountryNotFound"],
    },
  };

  componentDidMount() {
    const keyId = this.props.match.params.id;
    getKey(keyId)
      .then((response) => {
        if (response.data && response.data.succeeded) {
          this.setState({
            data: {
              name: response.data.value.name,
              country: response.data.value.country,
              serverAddress: response.data.value.serverAddress,
            },
          });
        }
      })
      .catch((error) => {
        if (error.response.status >= 500) window.location = "/internalError";
        const errors = this.validate(error.response.data);
        this.setState({ errors });
      });
    getServers()
      .then((response) => {
        if (response.data && response.data.succeeded) {
          console.log(this.state.data.country);
          this.setState({
            servers: response.data.value.filter(
              (s) =>
                s.country == this.state.data.country &&
                s.serverAddress != this.state.data.serverAddress
            ),
          });
        }
      })
      .catch((error) => {
        if (error.response.status >= 500) window.location = "/internalError";
        const errors = this.validate(error.response.data);
        this.setState({ errors });
      });
  }
  doRequest = async () => {
    const keyId = this.props.match.params.id;
    transferKeyToNewServer(keyId, this.state.data.serverId)
      .then((response) => {
        if (response.data && response.data.succeeded) {
          window.location = "/keys";
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
        <h1>Перенос ключа на новый сервер</h1>
        <form onSubmit={this.handleSubmit}>
          {this.renderSelect("serverId", "Сервер", this.state.servers)}
          {this.renderButton("Перенести")}
        </form>
      </div>
    );
  }
}

export default withRouter(TransferKeyForm);
