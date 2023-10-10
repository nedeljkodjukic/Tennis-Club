import Vue from "vue";
import Vuex from "vuex";

// import example from './module-example'

import auth from "./auth";
import apiRequest from "./apiRequest";

Vue.use(Vuex);

/*
 * If not building with SSR mode, you can
 * directly export the Store instantiation;
 *
 * The function below can be async too; either use
 * async/await or return a Promise which resolves
 * with the Store instance.
 */

const Store = new Vuex.Store({
  modules: { auth, apiRequest },
  strict: process.env.DEBUGGING
});

Store.subscribe((mutation, state) => {
  localStorage.setItem("tennisClubStore", JSON.stringify(state));
});

export default function(/* { ssrContext } */) {
  return Store;
}

export { Store };
