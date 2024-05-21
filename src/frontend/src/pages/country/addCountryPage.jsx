import { Component } from "react";
import AddCountryForm from "../../components/country/addCountryForm";
import auth from "../../services/authService";

class AddCountryPage extends Component {
  async componentDidMount() {
    if (!(await auth.hasRole("Admin"))) window.location = "/not-found";
  }

  render() {
    return <AddCountryForm />;
  }
}

export default AddCountryPage;
