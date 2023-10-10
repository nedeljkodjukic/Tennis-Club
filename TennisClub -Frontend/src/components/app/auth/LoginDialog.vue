<template>
  <q-dialog v-model="visible" persistent @hide="handleHide">
    <q-card class="q-py-sm full-width">
      <q-card-section class="row full-width justify-between items-center">
        <q-img src="~assets/logo2.png" width="60px" />
        <div class="text-h4 q-pl-sm text-primary">Prijava</div>
        <q-btn
          icon="close"
          flat
          round
          dense
          @click="handleHide"
          color="primary"
        />
      </q-card-section>
      <q-separator />
      <q-card-section>
        <q-form
          class="full-width column q-gutter-y-md q-pa-sm"
          @submit.prevent="handleLogin"
        >
          <q-input outlined placeholder="Email" v-model="email">
            <template v-slot:prepend>
              <q-icon name="person" />
            </template>
          </q-input>
          <q-input
            bottom-slots
            outlined
            placeholder="Šifra"
            :type="isPasswordVisible ? 'text' : 'password'"
            v-model="password"
          >
            <template v-slot:prepend>
              <q-icon name="lock" />
            </template>
            <template v-slot:append>
              <q-icon
                :name="isPasswordVisible ? 'visibility' : 'visibility_off'"
                class="cursor-pointer"
                @click="isPasswordVisible = !isPasswordVisible"
              />
            </template>
          </q-input>
          <q-btn
            class="q-py-sm"
            type="submit"
            color="primary"
            label="Prijavite se"
            no-caps
            :loading="loginButtonLoading"
          />
        </q-form>
      </q-card-section>
      <q-separator />
      <q-card-section>
        <div class="row q-gutter-x-sm justify-center">
          <div class="text-dark">
            Nemate nalog?
          </div>
          <a
            class="text-primary text-weight-bold"
            @click="handleRegisterClick"
            style="text-decoration:none;cursor: pointer"
          >
            Registrujte se
          </a>
        </div>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>

<script>
export default {
  name: "LoginDialog",
  props: {
    visible: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      email: "",
      password: "",
      toRemember: false,
      isPasswordVisible: false
    };
  },
  methods: {
    handleForgotPasswordClick() {
      this.$store.commit("auth/hideLoginDialog");
      this.$store.commit("auth/showForgotPasswordDialog");
    },
    handleRegisterClick() {
      this.$store.commit("auth/hideLoginDialog");
      this.$store.commit("auth/showRegisterDialog");
    },
    handleHide() {
      this.password = "";
      this.email = "";
      this.$store.commit("auth/hideLoginDialog");
      const query = Object.assign(this.$route.query);
      delete query.redirect;
      this.$router.replace({ query: {} });
    },
    handleLogin() {
      const payload = {
        email: this.email,
        password: this.password
      };

      this.$store
        .dispatch("auth/login", payload)
        .then(response => {
          if (this.$route.query.redirect) {
            this.$router.push(this.$route.query.redirect);
          }
          this.email = "";
          this.password = "";
        })
        .catch(error => {
          this.$q.notify({
            position: "top",
            message: error.data,
            type: "negative"
          });
        });
    }
  },
  computed: {
    loginButtonLoading() {
      return this.$store.state.auth.loading;
    }
  }
};
</script>

<style scoped></style>
