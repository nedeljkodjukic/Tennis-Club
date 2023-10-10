import axios from "axios";
import { baseUrl } from "src/config/apiConfig";
import { Notify } from "quasar";

const apiRequest = ({ commit }, { method, url, successMessage, data = {} }) => {
  return new Promise((resolve, reject) => {
    commit("showRequestLoading");
    axios
      .request({ method: method, url: url, baseURL: baseUrl, data: data })
      .then(response => {
        if (response.status === 200 && successMessage !== "") {
          Notify.create({
            type: "positive",
            position: "top",
            message: successMessage,
            timeout: 1500
          });
        }
        commit("hideRequestLoading");
        resolve(response.data);
      })
      .catch(error => {
        let errMsg = "";

        if (error.response) {
          if (error.response.data) {
            errMsg = error.response.data;
          } else {
            errMsg = error.response.statusText;
          }
        } else {
          errMsg = error.message;
        }

        Notify.create({
          type: "negative",
          position: "top",
          message: errMsg,
          timeout: 1000
        });
        commit("hideRequestLoading");
        reject(error.response);
      });
  });
};

export const getApiRequest = ({ commit }, { url, successMessage = "" }) =>
  apiRequest({ commit }, { method: "get", url, successMessage });
export const postApiRequest = (
  { commit },
  { url, data, successMessage = "" }
) => apiRequest({ commit }, { method: "post", url, successMessage, data });
export const patchApiRequest = (
  { commit },
  { url, data, successMessage = "" }
) => apiRequest({ commit }, { method: "patch", url, successMessage });
export const putApiRequest = ({ commit }, { url, data, successMessage = "" }) =>
  apiRequest({ commit }, { method: "put", url, successMessage, data });
export const deleteApiRequest = ({ commit }, { url, successMessage = "" }) =>
  apiRequest({ commit }, { method: "delete", url, successMessage });
