export const showLoginDialog = state => {
  state.showLoginDialog = true;
};

export const hideLoginDialog = state => {
  state.showLoginDialog = false;
};

export const showRegisterDialog = state => {
  state.showRegisterDialog = true;
};

export const hideRegisterDialog = state => {
  state.showRegisterDialog = false;
};

export const showForgotPasswordDialog = state => {
  state.showForgotPasswordDialog = true;
};

export const hideForgotPasswordDialog = state => {
  state.showForgotPasswordDialog = false;
};

export const loginRequest = state => {
  state.loading = true;
};

export const loginSuccess = (state, payload) => {
  state.auth = payload;
  state.loading = false;
};

export const loginError = state => {
  state.loading = false;
};

export const registerRequest = state => {
  state.loading = true;
};

export const registerSuccess = state => {
  state.loading = false;
};

export const registerError = state => {
  state.loading = false;
};

export const logout = state => {
  state.auth = null;
};

export const initialize = (state, payload) => {
  Object.assign(state, payload.auth);
};
