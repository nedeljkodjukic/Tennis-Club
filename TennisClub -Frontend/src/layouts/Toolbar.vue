<template>
  <q-toolbar
    class="col-xs-12 col-sm-10 col-md-10 col-lg-8 offset-sm-1 offset-md-1 offset-lg-2"
  >
    <q-img src="~assets/logo.png" width="70px" class="q-mr-md" />
    <q-separator vertical spaced color="white" />
    <navbar />
    <q-space />
    <div v-if="isAuthenticated" class="row items-center q-gutter-x-xl">
      <div class="text-weight-medium text-subtitle1">{{ fullName }}</div>
      <div>
        <div>
          <q-btn
            no-caps
            icon="logout"
            label="Odjavi se"
            flat
            dense
            @click="handleLogout"
          />
        </div>
      </div>
    </div>
    <div v-else class="q-gutter-md">
      <q-btn
        no-caps
        dense
        flat
        @click="handleLoginClick"
        icon="exit_to_app"
        label="Prijavi se"
      />
      <q-btn
        no-caps
        dense
        flat
        @click="handleRegisterClick"
        icon="person_add"
        label="Registruj se"
      />
    </div>
  </q-toolbar>
</template>

<script>
import Navbar from "layouts/Navbar";
export default {
  name: "Toolbar",
  components: { Navbar },
  data() {
    return {
      filterText: ""
    };
  },
  methods: {
    handleLogout() {
      this.$store.dispatch("auth/logout").then(() => {
        if (this.$route.meta.requiresAuth) {
          this.$router.push("/");
        }
      });
    },
    handleDrawerOpen() {
      this.$emit("drawerOpen");
    },
    handleLoginClick() {
      this.$store.commit("auth/showLoginDialog");
    },
    handleRegisterClick() {
      this.$store.commit("auth/showRegisterDialog");
    }
  },
  computed: {
    isAuthenticated() {
      return this.$store.getters["auth/isAuthenticated"];
    },
    fullName() {
      return this.$store.getters["auth/fullName"];
    }
  }
};
</script>

<style scoped></style>
