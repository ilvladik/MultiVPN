import React from "react";
import { withRouter } from "react-router-dom/cjs/react-router-dom.min";
import Form from "../common/form";
import { updateKey, getKey } from "../../services/keyService";

class UpdateKeyForm extends Form {
  state = {
    data: { name: "" },
    errors: {},
    propertyErrors: {},
  };

  componentDidMount() {
    const keyId = this.props.match.params.id;
    getKey(keyId)
      .then((response) => {
        if (response.data && response.data.succeeded) {
          this.setState({
            data: {
              name: response.data.value.name,
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
    updateKey(
      this.props.match.params.id,
      {
        name: this.state.data.name,
      },
      this.props.match.params.id
    )
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
        <h1>Обновление ключа</h1>
        <form onSubmit={this.handleSubmit}>
          {this.renderInput("name", "Название")}
          {this.renderButton("Обновить")}
        </form>
      </div>
    );
  }
}

export default withRouter(UpdateKeyForm);
