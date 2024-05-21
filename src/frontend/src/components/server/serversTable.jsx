import React, { Component } from "react";
import { Link } from "react-router-dom";
import Table from "../common/table";

class ServersTable extends Component {
  columns = [
    {
      path: "name",
      label: "Название",
      content: (server) => (
        <Link key={server.id} to={`/servers/${server.id}/update`}>
          {server.name}
        </Link>
      ),
    },
    { path: "apiUrl", label: "Api адрес" },
    { path: "country", label: "Страна" },
    {
      path: "isAvailable",
      label: "Доступен",
      content: (server) => (server.isAvailable ? "Да" : "Нет"),
    },
    { path: "keysCount", label: "Количество ключей" },
    {
      key: "delete",
      content: (server) => (
        <button
          onClick={() => this.props.onDelete(server)}
          className="btn btn-danger btn-sm"
        >
          Удалить
        </button>
      ),
    },
  ];

  render() {
    const { servers, onSort, sortColumn } = this.props;

    return (
      <Table
        columns={this.columns}
        data={servers}
        sortColumn={sortColumn}
        onSort={onSort}
      />
    );
  }
}

export default ServersTable;
