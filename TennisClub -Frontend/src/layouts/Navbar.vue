<template>
  <div class="q-gutter-x-lg row q-pl-md">
    <router-link
      v-for="link in navLinks"
      :key="link.name"
      class="text-body2 text-weight-medium"
      style="text-decoration:none"
      :to="link.route"
    >
      <span>{{ link.name | capText }}</span>
    </router-link>
  </div>
</template>

<script>
export default {
  name: "Navbar",
  data() {
    return {
      navLinks: [
        {
          name: "Igrači",
          route: "/player"
        },
        {
          name: "Rangiranje",
          route: "/ranking"
        },
        {
          name: "Direktni dueli",
          route: "/head2head"
        },
        {
          name: "Turniri",
          route: "/tournament"
        }
      ]
    };
  },
  computed: {
    numOfNavLinks() {
      return this.navLinks.length;
    }
  },
  filters: {
    capText(value) {
      return value.toUpperCase();
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
