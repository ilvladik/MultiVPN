import { Component } from "react";
import Countries from "../../components/country/countries";
import auth from "../../services/authService";

class CountriesPage extends Component {
  async componentDidMount() {
    if (!(await auth.hasRole("Admin"))) window.location = "/not-found";
  }

  render() {
    return <Countries />;
  }
}

export default CountriesPage;
