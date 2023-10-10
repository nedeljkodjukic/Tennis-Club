<template>
  <q-page>
    <q-form
      class="row q-py-md q-gutter-x-md full-width justify-center bg-secondary"
      @submit.prevent="handleTournamentSubmit"
    >
      <q-select
        style="min-width:210px"
        dense
        outlined
        color="primary"
        bg-color="white"
        v-model="selectedCategory"
        :options="categories"
        label="Kategorija"
        @input="val => handleTournamentOptions(val)"
      />
      <q-select
        style="min-width:210px"
        dense
        outlined
        color="primary"
        bg-color="white"
        v-model="selectedTournament"
        :options="tournamentOptions"
        label="Turnir"
      />
      <q-btn
        label="Prikaži"
        color="green-2"
        type="submit"
        no-caps
        icon="search"
        style="min-width:140px"
        :disable="!selectedTournament"
      />
    </q-form>

    <div class="row" v-if="current">
      <div
        class="col-xs-12 col-sm-10 col-md-10 col-lg-8 offset-sm-1 offset-md-1 offset-lg-2"
      >
        <div class="q-px-xl q-pt-lg full-width">
          <q-card class="full-width q-pa-none">
            <q-card-section class="column q-pa-none">
              <q-card-section class="row full-width items-center q-ma-sm">
                <div class="col-3">
                  <img style="height:100px" :src="current.logo" />
                </div>
                <div class="col-6 text-center text-h4 text-weight-bold">
                  {{ current.name }}
                </div>
                <div class="col-3 text-right">
                  <img
                    style="height:100px;border:1px
                  solid;border-color:#D3D3D3"
                    :src="getFlag(current.country.code)"
                  />
                </div>
              </q-card-section>
              <q-card-section class="row full-width justify-between q-pt-xl">
                <div
                  class="col-5 column text-h6 text-weight-light justify-between"
                >
                  <div class="text-subtitle2 text-weight-medium">
                    LOKACIJA:
                    {{
                      (current.city + ", " + current.country.name) | capString
                    }}
                  </div>
                  <q-separator />
                  <div class="text-subtitle2 text-weight-medium">
                    KATEGORIJA:
                    {{ current.category | capString }}
                  </div>
                  <q-separator />
                  <div class="text-subtitle2 text-weight-medium">
                    LOKACIJA:
                    {{
                      (current.city + ", " + current.country.name) | capString
                    }}
                  </div>
                  <q-separator />
                  <div class="text-subtitle2 text-weight-medium">
                    PODLOGA:
                    {{ current.surface | capString }}
                  </div>
                  <q-separator />
                  <div class="text-subtitle2 text-weight-medium">
                    NAJVIŠE TITULA:
                    {{ records[0].firstName | capString }}
                  </div>
                  <q-separator />
                  <div class="text-subtitle2 text-weight-medium">
                    POSLEDNJI ŠAMPION: {{ records[1].firstName | capString }}
                  </div>
                </div>
                <div
                  class="col-6 text-body2 text-justify text-italic text-weight-medium"
                >
                  <div class="text-h6 text-normal">O Turniru</div>
                  {{ current.tournamentInfo }}
                </div>
              </q-card-section>
            </q-card-section>
          </q-card>
        </div>
        <q-card class="q-mx-xl shadow-2">
          <q-tabs
            class="dense bg-secondary text-white"
            v-model="selectedTab"
            indicator-color="transparent"
            active-color="green-2"
            align="justify"
            inline-label
            @input="val => handleTabChange(val)"
          >
            <q-tab name="stadiums" icon="fas fa-info" label="Stadioni" />
            <q-tab name="winners" icon="fas fa-trophy" label="Pobednici" />
            <q-tab name="matches" icon="fas fa-running" label="Mečevi" />
          </q-tabs>
        </q-card>
        <q-tab-panels v-model="selectedTab" animated class="q-mx-xl">
          <q-tab-panel name="stadiums">
            <q-list class="full-width">
              <q-separator />
              <div v-for="item in current.courts" :key="item.playerId">
                <q-item>
                  <q-item-section>
                    <q-card side>
                      <img :src="item.picture" style="height:150px" />
                    </q-card>
                  </q-item-section>
                  <q-item-section class="text-h6 text-wight-medium text-center"
                    >IME: {{ item.name | capString }}
                  </q-item-section>
                  <q-item-section class="text-h6 text-wight-medium text-center"
                    >KAPACITET: {{ item.capacity }}
                  </q-item-section>
                </q-item>
                <q-separator />
              </div>
            </q-list>
          </q-tab-panel>
          <q-tab-panel name="winners">
            <q-list>
              <q-separator />
              <div v-for="item in winners" :key="item.playerId">
                <q-item>
                  <q-item-section
                    class="text-h6 text-weight-medium text-center"
                    >{{ item.year }}</q-item-section
                  >
                  <q-item-section
                    class="text-h6 text-weight-bold text-center"
                    >{{
                      item.player.firstName + " " + item.player.lastName
                    }}</q-item-section
                  >
                </q-item>
                <q-separator />
              </div>
            </q-list>
          </q-tab-panel>
          <q-tab-panel name="matches">
            <q-select
              style="min-width:210px"
              dense
              outlined
              color="primary"
              bg-color="white"
              v-model="selectedYear"
              :options="years"
              label="Godina"
              @input="val => handleMathesForYear(val)"
            />
            <matches-list :matches="matches" :title="selectedYear" />
          </q-tab-panel>
        </q-tab-panels>
      </div>
    </div>
  </q-page>
</template>

<script>
import MatchesList from "src/components/app/MatchesList.vue";
import { globalAppMixin } from "src/mixins/globalAppMixin";
export default {
  components: { MatchesList },
  name: "TournamentPage",
  mixins: [globalAppMixin],
  data() {
    return {
      categories: [],
      selectedCategory: null,
      tournaments: {},
      tournamentOptions: [],
      selectedTournament: null,
      current: null,
      records: [],
      selectedTab: "stadiums",
      winners: null,
      matches: null,
      years: [2021, 2020],
      selectedYear: 2021
    };
  },
  created() {
    this.$store
      .dispatch("apiRequest/getApiRequest", {
        url: `tournament/get_all_basic`
      })
      .then(response => {
        const groupedByCat = this.groupBy(response, "category");

        this.tournaments = { ...groupedByCat };

        this.categories = Object.keys(this.tournaments);
      });
  },
  methods: {
    handleTournamentOptions(category) {
      const t = this.tournaments[category];
      this.tournamentOptions = t.map(x => ({ value: x.id, label: x.name }));
    },
    handleTournamentSubmit() {
      this.matches = null;
      this.winners = null;
      this.$store
        .dispatch("apiRequest/getApiRequest", {
          url: `tournament/get/${this.selectedTournament.value}`
        })
        .then(response => {
          this.current = response;
        });

      this.$store
        .dispatch("apiRequest/getApiRequest", {
          url: `tournament/get_records/${this.selectedTournament.value}`
        })
        .then(response => {
          this.records = response;
        });
    },
    handleTabChange(tab) {
      if (tab === "winners" && !this.winners) {
        this.$store
          .dispatch("apiRequest/getApiRequest", {
            url: `tournament/get_past_winners/${this.selectedTournament.value}`
          })
          .then(response => {
            this.winners = response;
          });
      } else if (tab === "matches" && !this.matches) {
        this.handleMathesForYear(new Date().getFullYear());
      }
    },
    handleMathesForYear(val) {
      this.$store
        .dispatch("apiRequest/getApiRequest", {
          url: `tournament/get_matches/${this.selectedTournament.value}/${val}`
        })
        .then(response => {
          this.matches = response;
        });
    },
    groupBy(xs, key) {
      return xs.reduce(function(rv, x) {
        (rv[x[key]] = rv[x[key]] || []).push(x);
        return rv;
      }, {});
    }
  }
};
</script>
