import axios from "axios";
import logger from "./logService";

axios.interceptors.response.use(
  response => {
    return response;
  },
  error => {
    const expectedError = false;
      // error.response &&
      // error.response.status >= 400 &&
      // error.response.status < 500;

    if (!expectedError) {
      console.error('Unexpected error:', error);
    }

    return Promise.reject(error);
  }
);

function setJwt(jwt) {
  axios.defaults.headers.common["Authorization"] = "Bearer " + jwt;
}

export default {
  get: axios.get,
  post: axios.post,
  put: axios.put,
  delete: axios.delete,
  setJwt
};
