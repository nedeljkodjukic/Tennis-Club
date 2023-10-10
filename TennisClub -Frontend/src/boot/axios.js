import Vue from "vue";
import axios from "axios";
import { Store } from "src/store";

Vue.prototype.$axios = axios;

const initialStore = localStorage.getItem("tennisClubStore");

if (initialStore) {
  Store.commit("auth/initialize", JSON.parse(initialStore));
  if (Store.getters["auth/isAuthenticated"]) {
    axios.defaults.headers.common.Authorization = `Bearer ${Store.state.auth.auth.accessToken}`;
  }
}
