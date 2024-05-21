import { Component } from "react";
import UpdateServerForm from "../../components/server/updateServerForm";
import TrasferKeysForm from "../../components/server/transferKeysForm";
import Keys from "../../components/key/keys";
import auth from "../../services/authService";

class UpdateServerPage extends Component {
  async componentDidMount() {
    if (!(await auth.hasRole("Admin"))) window.location = "/not-found";
  }

  render() {
    return (
      <div>
        <UpdateServerForm />
        <br />
        <TrasferKeysForm />
        <br />
        <Keys />
      </div>
    );
  }
}

export default UpdateServerPage;
