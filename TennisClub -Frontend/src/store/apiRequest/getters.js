export const isLoading = state => {
  return state.numberOfApiRequestsInProgress > 0;
};
