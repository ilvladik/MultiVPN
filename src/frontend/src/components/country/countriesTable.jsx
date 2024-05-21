import React, { Component } from "react";
import Table from "../common/table";

class CountriesTable extends Component {
  columns = [
    {
      path: "name",
      label: "Название",
      content: (country) => <div>{country.name}</div>,
    },
    {
      key: "delete",
      content: (country) => (
        <button
          onClick={() => this.props.onDelete(country)}
          className="btn btn-danger btn-sm"
        >
          Удалить
        </button>
      ),
    },
  ];

  render() {
    const { countries, onSort, sortColumn } = this.props;

    return (
      <Table
        columns={this.columns}
        data={countries}
        sortColumn={sortColumn}
        onSort={onSort}
      />
    );
  }
}

export default CountriesTable;
