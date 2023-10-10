import VueJwtDecode from "vue-jwt-decode";

export const isAuthenticated = state => {
  return (
    state.auth !== null &&
    state.auth.accessToken !== null &&
    new Date(state.auth.expires) > new Date()
  );
};

export const toShowLogin = state => {
  return state.showLoginDialog;
};

export const isInRoles = (state, getters) => roles => {
  for (let i = 0; i < roles.length; i++) {
    var role = roles[i];
    if (
      !(
        state.auth !== null &&
        state.auth.accessToken !== null &&
        VueJwtDecode.decode(state.auth.accessToken).role.includes(role)
      )
    ) {
      return false;
    }
  }
  return true;
};

export const fullName = state => {
  return `${state.auth.name} ${state.auth.surname}`;
};

export const userName = state => {
  return VueJwtDecode.decode(state.auth.accessToken).unique_name;
};
