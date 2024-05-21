import React, { Component } from "react";
import { withRouter } from "react-router-dom/cjs/react-router-dom.min";
import { Link } from "react-router-dom";
import KeysTable from "./keysTable";
import Pagination from "../common/pagination";
import { getKeys, deleteKey } from "../../services/keyService";
import { paginate } from "../../utils/paginate";
import _ from "lodash";
import SearchBox from "../common/searchBox";

class Keys extends Component {
  state = {
    keys: [],
    currentPage: 1,
    pageSize: 5,
    searchQuery: "",
    sortColumn: { path: "name", order: "asc" },
    errors: [],
  };

  componentDidMount() {
    getKeys()
      .then((response) => {
        if (response.data && response.data.succeeded) {
          const keys = response.data.value;
          if (this.state.serverAddress)
            keys.filter((k) => k.serverAddress == this.state.serverAddress);
          this.setState({ keys });
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

  handleDelete = async (key) => {
    const originalKeys = this.state.keys;
    const keys = originalKeys.filter((s) => s.id !== s.id);
    this.setState({ keys });

    deleteKey(key.id)
      .then((_) => {})
      .catch((error) => {
        this.setState({
          errors: this.validateDelete(error).map((error) => (
            <div className="alert alert-danger">{error}</div>
          )),
        });
        this.setState({ keys: originalKeys });
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
      keys: allKeys,
    } = this.state;

    let filtered = allKeys;
    if (searchQuery)
      filtered = allKeys.filter((s) =>
        s.name.toLowerCase().startsWith(searchQuery.toLowerCase())
      );

    const sorted = _.orderBy(filtered, [sortColumn.path], [sortColumn.order]);

    const keys = paginate(sorted, currentPage, pageSize);

    return { totalCount: filtered.length, data: keys };
  };

  render() {
    const { length: count } = this.state.keys;
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
              Новая ключ
            </Link>
            <div>Ни один ключ ещё не был добавлен</div>
          </div>
        </div>
      );

    const { totalCount, data: keys } = this.getPagedData();

    return (
      <div className="row">
        <div className="col">
          <Link
            to="/keys/new"
            className="btn btn-primary"
            style={{ marginBottom: 20 }}
          >
            Новый ключ
          </Link>
          <SearchBox value={searchQuery} onChange={this.handleSearch} />
          {this.state.errors}
          <KeysTable
            keys={keys}
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

export default withRouter(Keys);
