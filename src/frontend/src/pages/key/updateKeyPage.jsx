import { Component } from "react";
import TrasferKeyForm from "../../components/key/transferKeyForm";
import auth from "../../services/authService";
import UpdateKeyForm from "../../components/key/updateKeyForm";

class UpdateKeyPage extends Component {
  state = {
    isAdmin: false,
  };
  async componentDidMount() {
    auth.hasRole("Admin").then((value) => {
      this.state.isAdmin = value;
    });
  }

  render() {
    return (
      <div>
        <UpdateKeyForm />
        <br />
        {this.state.isAdmin ?? <TrasferKeyForm />}
      </div>
    );
  }
}

export default UpdateKeyPage;
