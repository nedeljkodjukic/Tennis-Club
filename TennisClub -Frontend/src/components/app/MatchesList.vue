<template>
  <div class="q-pa-md column items-center full-width">
    <div class="text-h6 text-weight-medium q-pb-sm">{{ title }}</div>
    <q-list class="full-width">
      <q-separator />
      <div v-for="item in matches" :key="item.id">
        <q-expansion-item>
          <template v-slot:header>
            <q-item-section side>
              <div class="text-subtitle2">
                {{ item.date | formatDate }}
              </div>
            </q-item-section>
            <q-item-section>
              <div class="row justify-center text-weight-medium">
                {{ item.firstPlayerFullName }} - {{ item.secondPlayerFullName }}
              </div>
            </q-item-section>

            <q-item-section side>
              <div class="text-subtitle2 text-weight-bold">
                {{ item.result }}
              </div>
            </q-item-section>
          </template>
          <q-separator />
          <q-card>
            <q-card-section class="column full-width">
              <div
                v-for="(set, index) in item.sets"
                :key="index"
                class="row full-width"
              >
                <div class="col-4"></div>
                <div
                  class="col-4 row justify-center text-subtitle1 text-weight-bold"
                >
                  {{ set.firstPlayerScore }} - {{ set.secondPlayerScore }}
                </div>
                <div
                  class="col-4 row justify-end text-subtitle2 text-weight-medium"
                >
                  {{ set.duration }}'
                </div>
              </div>
              <div
                class="row justify-between q-pt-md text-subtitle2 text-weight-medium"
              >
                <div>Teren: {{ item.courtName }}</div>
                <div>Turnir: {{ item.tournamentName }}</div>
              </div>
              <div
                class="row justify-between text-subtitle2 text-weight-medium"
              >
                <div>Poseta: {{ item.attendance }}</div>
                <div>Faza: {{ item.phase }}</div>
              </div>
            </q-card-section>
          </q-card>
        </q-expansion-item>
        <q-separator />
      </div>
    </q-list>
  </div>
</template>

<script>
import moment from "moment";
export default {
  name: "MatchesList",
  props: {
    matches: {
      type: Array,
      required: true
    },
    title: {
      type: String,
      required: false,
      default: "Prethodni mečevi"
    }
  },
  data() {
    return {};
  },
  methods: {},
  filters: {
    formatDate: function(value) {
      if (value) {
        return moment(String(value)).format("DD.MM.YYYY");
      }
    }
  }
};
</script>
