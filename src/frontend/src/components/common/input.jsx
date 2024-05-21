import React from "react";

const Input = ({ name, label, errors = [], ...rest }) => {
  errors = errors.map((error) => (
    <div className="alert alert-danger">{error}</div>
  ));
  const result =
    rest["type"] && rest["type"] == "checkbox" ? (
      <div className="form-group">
        <label htmlFor={name}>{label}</label>
        <input {...rest} name={name} id={name} />
        {errors}
      </div>
    ) : (
      <div className="form-group">
        <label htmlFor={name}>{label}</label>
        <input {...rest} name={name} id={name} className="form-control" />
        {errors}
      </div>
    );
  return result;
};

export default Input;
