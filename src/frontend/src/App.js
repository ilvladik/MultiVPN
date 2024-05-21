import React, { Component } from "react";
import { Route, Switch } from "react-router-dom";
import { Redirect } from "react-router-dom/cjs/react-router-dom.min";
import NotFound from "./components/notFound";
import Forbidden from "./components/forbidden";
import InternalServerError from "./components/internalServerError";
import NavBar from "./components/navBar";
import RegisterForm from "./components/auth/registerForm";
import LoginForm from "./components/auth/loginForm";
import Logout from "./components/auth/logout";
import ForgotPasswordForm from "./components/auth/forgotPasswordForm";
import ResetPasswordForm from "./components/auth/resetPasswordForm";
import SuccessForgotPassword from "./components/auth/successForgotPassword";
import SuccessRegistration from "./components/auth/successRegistration";

import CountriesPage from "./pages/country/countriesPage";
import AddCountryPage from "./pages/country/addCountryPage";

import ServersPage from "./pages/server/serversPage";
import UpdateServerPage from "./pages/server/updateServerPage";
import AddServerPage from "./pages/server/addServerPage";

import KeysPage from "./pages/key/keysPage";
import AddKeyPage from "./pages/key/addKeyPage";
import UpdateKeyPage from "./pages/key/updateKeyPage";

import "./App.css";


const App = () => {
  

  return (
      <React.Fragment>
        <NavBar />
        <main className="container">
          <Switch>
            <Route path="/register" component={RegisterForm} />
            <Route path="/login" component={LoginForm} />
            <Route path="/logout" component={Logout} />
            <Route path="/successForgotPassword" component={SuccessForgotPassword} />
            <Route path="/successRegistration" component={SuccessRegistration} />
            <Route path="/forgotPassword" component={ForgotPasswordForm} />
            <Route path="/resetPassword" component={ResetPasswordForm} />
            
            <Route exact path="/countries" component={CountriesPage} />
            <Route exact path="/countries/new" component={AddCountryPage}/>

            <Route path="/servers/:id/update" component={UpdateServerPage} />
            <Route exact path="/servers/new" component={AddServerPage} />
            <Route exact path="/servers" component={ServersPage} />

            <Route exact path="/keys/new" component={AddKeyPage} />
            <Route path="/keys/:id/update" component={UpdateKeyPage} />
            <Route exact path="/keys" component={KeysPage} />
            
            <Route path="/not-found" component={NotFound} />
            <Route path="/forbidden" component={Forbidden} />
            <Route path="/internalError" component={InternalServerError} />
            <Redirect to="/" />
          </Switch>
        </main>
      </React.Fragment>
    );
};

export default App;