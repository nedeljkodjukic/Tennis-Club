<template>
  <q-dialog v-model="visible" persistent @hide="handleHide">
    <q-card class="q-py-sm full-width text-primary">
      <q-card-section class="row full-width justify-between items-center">
        <q-img src="~assets/logo2.png" width="60px" />
        <div class="text-h4 q-pl-sm">Registracija</div>
        <q-btn icon="close" flat round dense @click="handleHide" />
      </q-card-section>
      <q-separator />
      <q-card-section>
        <q-form
          ref="form"
          class="full-width column q-gutter-y-md"
          @submit.prevent="handleRegister"
        >
          <q-input dense outlined v-model="email" label="Email">
            <template v-slot:prepend>
              <q-icon name="email" />
            </template>
          </q-input>
          <q-input
            dense
            outlined
            v-model="password"
            type="password"
            label="Šifra"
          >
            <template v-slot:prepend>
              <q-icon name="lock" />
            </template>
          </q-input>
          <q-input
            dense
            outlined
            v-model="confirmPassword"
            type="password"
            label="Potvrdi šifru"
            :rules="[val => val === password || 'Ne poklapa se']"
          >
            <template v-slot:prepend>
              <q-icon name="lock" />
            </template>
          </q-input>
          <q-input v-model="userName" label="Korisnicko ime" dense outlined />
          <q-input v-model="firstName" label="Ime" dense outlined />
          <q-input v-model="lastName" label="Prezime" dense outlined />
          <q-btn
            class="q-py-sm"
            type="submit"
            color="primary"
            label="Pegistrujte se"
            no-caps
            :loading="registerButtonLoading"
          />
        </q-form>
      </q-card-section>
      <q-separator />
      <q-card-section>
        <div class="row q-gutter-x-sm justify-center">
          <div class="text-dark">
            Imate nalog?
          </div>
          <a
            class="text-primary text-weight-bold"
            @click="handleLoginClick"
            style="text-decoration:none;cursor: pointer"
          >
            Prijavite se
          </a>
        </div>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>

<script>
export default {
  name: "RegisterDialog",
  components: {},
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
      confirmPassword: "",
      userName: "",
      firstName: "",
      lastName: ""
    };
  },
  methods: {
    handleHide() {
      this.$store.commit("auth/hideRegisterDialog");
      const query = Object.assign(this.$route.query);
      delete query.redirect;
      this.$router.push({ query: query });
      this.$refs.stepper.goTo(1);
    },
    handleLoginClick() {
      this.$store.commit("auth/hideRegisterDialog");
      this.$store.commit("auth/showLoginDialog");
    },
    handleRegister() {
      const payload = { ...this.$data };

      debugger;
      this.$store
        .dispatch("auth/register", payload)
        .then(response => {
          this.$q.notify({
            position: "top",
            message: "Uspešno ste se registrovali",
            type: "positive"
          });
        })
        .catch(response => {
          debugger;
          this.$q.notify({
            position: "top",
            message: response.data,
            type: "negative"
          });
        });
    }
  },
  computed: {
    registerButtonLoading() {
      return this.$store.state.auth.loading;
    }
  }
};
</script>

<style scoped></style>
