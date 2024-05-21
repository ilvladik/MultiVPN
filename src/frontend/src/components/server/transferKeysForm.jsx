import React from "react";
import { withRouter } from "react-router-dom/cjs/react-router-dom.min";
import Form from "../common/form";
import { getServer, getServers } from "../../services/serverService";
import { transferKeysToNewServer } from "../../services/keyService";

class TransferKeysForm extends Form {
  state = {
    data: { name: "", serverAddress: "", country: "", serverId: "" },
    errors: {},
    servers: [],
    propertyErrors: {
      serverId: [],
    },
  };

  componentDidMount() {
    const serverId = this.props.match.params.id;
    getServer(serverId)
      .then((response) => {
        if (response.data && response.data.succeeded) {
          this.setState({
            data: {
              name: response.data.value.name,
              serverAddress: response.data.value.serverAddress,
              country: response.data.value.country,
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
          this.setState({
            servers: response.data.value.filter(
              (s) => s.country == this.state.data.country
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
    const serverId = this.props.match.params.id;
    transferKeysToNewServer(serverId, this.state.data.serverId)
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
        <h1>Перенос ключей на другой сервер</h1>
        <form onSubmit={this.handleSubmit}>
          {this.renderSelect("serverId", "Сервер", this.state.servers)}
          {this.renderButton("Перенести")}
        </form>
      </div>
    );
  }
}

export default withRouter(TransferKeysForm);
