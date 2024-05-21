import { Component } from "react";
import AddServerForm from "../../components/server/addServerForm";
import auth from "../../services/authService";

class AddServerPage extends Component {
  async componentDidMount() {
    if (!(await auth.hasRole("Admin"))) window.location = "/not-found";
  }

  render() {
    return <AddServerForm />;
  }
}

export default AddServerPage;
