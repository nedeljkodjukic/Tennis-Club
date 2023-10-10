import axios from "axios";
import { baseUrl } from "src/config/apiConfig";

export const login = ({ commit }, payload) => {
  return new Promise((resolve, reject) => {
    commit("loginRequest");
    axios
      .request({
        method: "post",
        url: "identity/login",
        baseURL: baseUrl,
        data: payload
      })
      .then(response => {
        const auth = response.data;
        axios.defaults.headers.common.Authorization = `Bearer ${auth.accessToken}`;
        commit("loginSuccess", auth);
        commit("hideLoginDialog");
        resolve(response);
      })
      .catch(error => {
        commit("loginError");
        delete axios.defaults.headers.common.Authorization;
        reject(error.response);
      });
  });
};

export const register = ({ commit }, payload) => {
  return new Promise((resolve, reject) => {
    commit("registerRequest");
    axios
      .request({
        method: "post",
        url: "identity/register/2",
        baseURL: baseUrl,
        data: payload
      })
      .then(response => {
        commit("registerSuccess");
        commit("hideRegisterDialog");
        resolve(response);
      })
      .catch(error => {
        commit("registerError");
        reject(error.response);
      });
  });
};

export const logout = ({ commit }) => {
  commit("logout");
  delete axios.defaults.headers.common.Authorization;
};
