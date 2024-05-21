import React from "react";
import Form from "../common/form";
import { withRouter } from "react-router-dom/cjs/react-router-dom.min";
import { updateServer, getServer } from "../../services/serverService";

class UpdateServerForm extends Form {
  state = {
    data: { name: "", isAvailable: false },
    errors: {},
    propertyErrors: {},
  };

  componentDidMount() {
    const serverId = this.props.match.params.id;
    getServer(serverId)
      .then((response) => {
        if (response.data && response.data.succeeded) {
          this.setState({
            data: {
              name: response.data.value.name,
              isAvailable: response.data.value?.isAvailable,
            },
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
    updateServer(
      {
        name: this.state.data.name,
        isAvailable: this.state.data.isAvailable,
      },
      this.props.match.params.id
    )
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
        <h1>Обновление сервера</h1>
        <form onSubmit={this.handleSubmit}>
          {this.renderInput("name", "Название")}
          <br />
          {this.renderInput(
            "isAvailable",
            "Доступен",
            "checkbox",
            this.state.data.isAvailable
          )}
          {this.renderButton("Обновить")}
        </form>
      </div>
    );
  }
}

export default withRouter(UpdateServerForm);
