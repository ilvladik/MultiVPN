import React, { Component } from "react";
import { Link } from "react-router-dom";
import CountriesTable from "./countriesTable";
import Pagination from "../common/pagination";
import { getCountries, deleteCountry } from "../../services/countryService";
import { paginate } from "../../utils/paginate";
import _ from "lodash";
import SearchBox from "../common/searchBox";

class Countries extends Component {
  state = {
    countries: [],
    currentPage: 1,
    pageSize: 5,
    searchQuery: "",
    sortColumn: { path: "name", order: "asc" },
  };

  componentDidMount() {
    getCountries()
      .then((response) => {
        if (response.data && response.data.succeeded) {
          const countries = response.data.value;
          this.setState({ countries });
        }
      })
      .catch((error) => {
        if (error.response.status >= 500) window.location = "/internalError";
      });
  }

  handleDelete = async (country) => {
    const originalCountries = this.state.countries;
    const countries = originalCountries.filter((c) => c.id !== country.id);
    this.setState({ countries });

    deleteCountry(country.id)
      .then((_) => {})
      .catch((error) => {
        this.setState({ countries: originalCountries });
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
      countries: allCountries,
    } = this.state;

    let filtered = allCountries;
    if (searchQuery)
      filtered = allCountries.filter((c) =>
        c.name.toLowerCase().startsWith(searchQuery.toLowerCase())
      );

    const sorted = _.orderBy(filtered, [sortColumn.path], [sortColumn.order]);

    const countries = paginate(sorted, currentPage, pageSize);

    return { totalCount: filtered.length, data: countries };
  };

  render() {
    const { length: count } = this.state.countries;
    const { pageSize, currentPage, sortColumn, searchQuery } = this.state;

    if (count === 0)
      return (
        <div className="row">
          <div className="col">
            <Link
              to="/countries/new"
              className="btn btn-primary"
              style={{ marginBottom: 20 }}
            >
              Новая страна
            </Link>
            <div>Ни одна страна ещё не была добавлена</div>
          </div>
        </div>
      );

    const { totalCount, data: countries } = this.getPagedData();

    return (
      <div className="row">
        <div className="col">
          <Link
            to="/countries/new"
            className="btn btn-primary"
            style={{ marginBottom: 20 }}
          >
            Новая страна
          </Link>
          <SearchBox value={searchQuery} onChange={this.handleSearch} />
          <CountriesTable
            countries={countries}
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

export default Countries;
