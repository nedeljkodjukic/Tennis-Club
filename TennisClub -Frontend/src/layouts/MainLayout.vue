<template>
  <q-layout view="hHh lpR fFf">
    <login-dialog :visible="showLoginDialog" />
    <register-dialog :visible="showRegisterDialog" />
    <q-header elevated class="row q-py-sm">
      <toolbar />
    </q-header>
    <q-page-container>
      <router-view class="bg-white col-12" />
    </q-page-container>
    <q-footer elevated>
      <div class="row justify-center q-pa-sm">
        <div>
          {{ "Tennis Club ® 2023" }}
        </div>
      </div>
    </q-footer>
  </q-layout>
</template>

<script>
import Toolbar from "layouts/Toolbar";
import LoginDialog from "components/app/auth/LoginDialog";
import RegisterDialog from "components/app/auth/RegisterDialog";

export default {
  name: "MainLayout",
  components: { Toolbar, LoginDialog, RegisterDialog },
  data() {
    return {};
  },
  computed: {
    showLoadingBar() {
      return this.$store.getters["apiRequest/isLoading"];
    },
    showLoginDialog() {
      return this.$store.getters["auth/toShowLogin"];
      // return this.$store.state.auth.showLoginDialog
    },
    showRegisterDialog() {
      return this.$store.state.auth.showRegisterDialog;
    }
  }
};
</script>
