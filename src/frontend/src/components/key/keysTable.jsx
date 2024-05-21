import React, { Component } from "react";
import auth from "../../services/authService";
import { Link } from "react-router-dom";
import Table from "../common/table";

class KeysTable extends Component {
  columns = [
    {
      path: "name",
      label: "Название",
      content: (key) => (
        <Link key={key.id} to={`/keys/${key.id}/update`}>
          {key.name ? key.name : "Без названия"}
        </Link>
      ),
    },
    { path: "accessUri", label: "Адрес доступа" },
    { path: "country", label: "Страна" },
    {
      key: "delete",
      content: (key) => (
        <button
          onClick={() => this.props.onDelete(key)}
          className="btn btn-danger btn-sm"
        >
          Удалить
        </button>
      ),
    },
  ];

  render() {
    const { keys, onSort, sortColumn } = this.props;

    return (
      <Table
        columns={this.columns}
        data={keys}
        sortColumn={sortColumn}
        onSort={onSort}
      />
    );
  }
}

export default KeysTable;
