import { Component } from "react";
import Keys from "../../components/key/keys";
import auth from "../../services/authService";

class KeysPage extends Component {
  async componentDidMount() {
    if (!(await auth.isAuthenticated())) window.location = "/not-found";
  }

  render() {
    return <Keys />;
  }
}

export default KeysPage;
