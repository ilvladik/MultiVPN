import React, { Component } from "react";
import Input from "./input";
import Select from "./select";

class Form extends Component {
  state = {
    data: {},
    errors: {},
    propertyErrors: {},
  };

  validate = (result) => {
    if (result.succeeded) {
      return {};
    }
    const value = {};
    const errors = result.errors;
    for (const prop in this.state.propertyErrors) {
      const errorCodes = this.state.propertyErrors[prop];
      value[prop] = errorCodes
        .map((errorCode) => {
          const error = errors.find((e) => e.code === errorCode);
          return error ? error.description : null;
        })
        .filter((error) => error !== null);
    }
    return value;
  };

  handleSubmit = (e) => {
    e.preventDefault();
    this.doRequest();
  };

  handleChange = (event) => {
    const { name, type, checked, value } = event.target;
    const data = { ...this.state.data };
    data[name] = type === "checkbox" ? checked : value;
    this.setState({ data });
  };

  renderButton(label) {
    return <button className="btn btn-primary">{label}</button>;
  }

  renderSelect(name, label, options) {
    const { data, errors } = this.state;

    return (
      <Select
        name={name}
        value={data[name]}
        label={label}
        options={options}
        onChange={this.handleChange}
        error={errors[name]}
      />
    );
  }

  renderInput(name, label, type = "text", checked = false) {
    const { data, errors } = this.state;
    return (
      <Input
        type={type}
        name={name}
        value={data[name]}
        label={label}
        onChange={this.handleChange}
        checked={checked}
        errors={errors[name]}
      />
    );
  }
}

export default Form;
