import getFlagUrl from "assets/flags.js";
import moment from "moment";

export const globalAppMixin = {
  methods: {
    getFlag(countryCode) {
      return getFlagUrl(countryCode);
    }
  },
  filters: {
    formatDate: function(value) {
      if (value) {
        return moment(String(value)).format("DD.MM.YYYY");
      }
    },
    capString: function(value) {
      if (value) {
        return value.toUpperCase();
      }
    }
  }
};
