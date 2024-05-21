import { Component } from "react";
import Servers from "../../components/server/servers";
import auth from "../../services/authService";

class ServersPage extends Component {
  async componentDidMount() {
    if (!(await auth.hasRole("Admin"))) window.location = "/not-found";
  }

  render() {
    return <Servers />;
  }
}

export default ServersPage;
