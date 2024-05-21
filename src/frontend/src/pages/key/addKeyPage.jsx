import { Component } from "react";
import AddKeyForm from "../../components/key/addKeyForm";
import auth from "../../services/authService";

class AddKeyPage extends Component {
  async componentDidMount() {
    if (!(await auth.isAuthenticated())) window.location = "/not-found";
  }

  render() {
    return <AddKeyForm />;
  }
}

export default AddKeyPage;
