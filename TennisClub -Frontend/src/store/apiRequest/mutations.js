export const showRequestLoading = state => {
  state.numberOfApiRequestsInProgress++;
};

export const hideRequestLoading = state => {
  state.numberOfApiRequestsInProgress--;
};
