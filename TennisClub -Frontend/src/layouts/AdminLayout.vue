<template>
  <q-layout view="hHh lpR fFf">
    <q-header elevated class="row q-py-sm">
      <div class="row q-gutter-x-lg row q-px-xl items-center">
        <router-link
          class="text-body2 text-weight-medium"
          style="text-decoration:none"
          to="/administration/players"
          ><span>IGRAČI</span>
        </router-link>
        <router-link
          class="text-body2 text-weight-medium"
          style="text-decoration:none"
          to="/administration/tournaments"
        >
          <span>TURNIRI</span>
        </router-link>
      </div>
      <div>
        <q-btn icon="logout" flat round dense @click="handleLogout" />
      </div>
    </q-header>
    <q-page-container class="row">
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
export default {
  name: "AdminLayout",
  components: {},
  data() {
    return {};
  },
  methods: {
    handleLogout() {
      this.$store.dispatch("auth/logout").then(() => {
        debugger;
        if (this.$route.meta.requiresAuth) {
          this.$router.push("/");
        }
      });
    }
  }
};
</script>

<style scoped lang="scss">
span {
  padding-bottom: 5px;
  color: white;
  border-bottom-style: hidden;
  border-width: 5px;
  border-color: $accent;
}

span:hover {
  border-bottom-style: solid;
  transition: 0.1s;
}
</style>
