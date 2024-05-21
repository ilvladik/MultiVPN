import React, { Component } from "react";
import { Link } from "react-router-dom";
import ServersTable from "./serversTable";
import Pagination from "../common/pagination";
import { getServers, deleteServer } from "../../services/serverService";
import { paginate } from "../../utils/paginate";
import _ from "lodash";
import SearchBox from "../common/searchBox";

class Servers extends Component {
  state = {
    servers: [],
    currentPage: 1,
    pageSize: 5,
    searchQuery: "",
    sortColumn: { path: "keyCount", order: "asc" },
    errors: [],
  };

  componentDidMount() {
    getServers()
      .then((response) => {
        if (response.data && response.data.succeeded) {
          const servers = response.data.value;
          this.setState({ servers });
        }
      })
      .catch((error) => {
        if (error.response.status >= 500) window.location = "/500";
      });
  }

  validateDelete = (error) => {
    if (error.response && error.response.data && error.response.data.errors) {
      const errors = error.response.data.errors.map((e) => e.description);
      return errors;
    }
    return [];
  };

  handleDelete = async (server) => {
    const originalServers = this.state.servers;
    const servers = originalServers.filter((s) => s.id !== s.id);
    this.setState({ servers });

    deleteServer(server.id)
      .then((_) => {})
      .catch((error) => {
        this.setState({
          errors: this.validateDelete(error).map((error) => (
            <div className="alert alert-danger">{error}</div>
          )),
        });
        this.setState({ servers: originalServers });
      });
  };

  handlePageChange = (page) => {
    this.setState({ currentPage: page });
  };

  handleSearch = (query) => {
    this.setState({ searchQuery: query, currentPage: 1 });
  };

  handleSort = (sortColumn) => {
    this.setState({ sortColumn });
  };

  getPagedData = () => {
    const {
      pageSize,
      currentPage,
      sortColumn,
      searchQuery,
      servers: allServers,
    } = this.state;

    let filtered = allServers;
    if (searchQuery)
      filtered = allServers.filter((s) =>
        s.name.toLowerCase().startsWith(searchQuery.toLowerCase())
      );

    const sorted = _.orderBy(filtered, [sortColumn.path], [sortColumn.order]);

    const servers = paginate(sorted, currentPage, pageSize);

    return { totalCount: filtered.length, data: servers };
  };

  render() {
    const { length: count } = this.state.servers;
    const { pageSize, currentPage, sortColumn, searchQuery } = this.state;

    if (count === 0)
      return (
        <div className="row">
          <div className="col">
            <Link
              to="/servers/new"
              className="btn btn-primary"
              style={{ marginBottom: 20 }}
            >
              Новая сервер
            </Link>
            <div>Ни один сервер ещё не был добавлен</div>
          </div>
        </div>
      );

    const { totalCount, data: servers } = this.getPagedData();

    return (
      <div className="row">
        <div className="col">
          <Link
            to="/servers/new"
            className="btn btn-primary"
            style={{ marginBottom: 20 }}
          >
            Новый сервер
          </Link>
          <SearchBox value={searchQuery} onChange={this.handleSearch} />
          {this.state.errors}
          <ServersTable
            servers={servers}
            sortColumn={sortColumn}
            onDelete={this.handleDelete}
            onSort={this.handleSort}
          />
          <Pagination
            itemsCount={totalCount}
            pageSize={pageSize}
            currentPage={currentPage}
            onPageChange={this.handlePageChange}
          />
        </div>
      </div>
    );
  }
}

export default Servers;
