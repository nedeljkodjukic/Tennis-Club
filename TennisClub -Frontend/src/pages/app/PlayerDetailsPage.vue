<template>
  <q-page>
    <q-form
      class="row q-py-md full-width justify-center bg-secondary items-center"
    >
      <q-select
        style="min-width:250px"
        dense
        outlined
        color="primary"
        bg-color="white"
        v-model="selectedPlayer"
        :options="playersList"
        label="Igrač"
        @input="value => handleGetPlayer()"
      />
    </q-form>
    <div v-if="selectedPlayer" class="row">
      <div
        class="col-xs-12 col-sm-10 col-md-10 col-lg-8 offset-sm-1 offset-md-1 offset-lg-2"
      >
        <div class="q-px-xl q-pt-lg full-width">
          <q-card class="full-width q-pa-none">
            <q-card-section class="row justify-between q-pa-none">
              <q-card-section class="column col-3 justify-between q-ma-sm">
                <div>
                  <div class="text-h4 text-weight-bold">
                    # {{ this.playerInfo.rank }}
                  </div>
                  <div>
                    <div class="text-h4 text-weight-medium q-pt-sm">
                      {{ this.playerInfo.firstName | capString }}
                    </div>
                    <div class="text-h3 text-weight-bolder">
                      {{ this.playerInfo.lastName | capString }}
                    </div>
                  </div>
                </div>
                <div>
                  <doughnut-chart
                    :data="[playerInfo.totalLosses, playerInfo.totalWins]"
                    :height="140"
                    :width="140"
                    :colors="['#9aaebb', '#0094C6']"
                    :labels="['Pobede', 'Porazi']"
                  />
                  <div class="text-h5 text-weight-bold text-center q-mt-xs">
                    {{ playerInfo.titles }}
                    <i class="fas fa-trophy" />
                  </div>
                </div>
              </q-card-section>
              <q-card-section class="column justify-between col-3 q-ma-sm">
                <div>
                  <img
                    :src="getFlag(playerInfo.country.code)"
                    style="height:100px;border:1px solid; border-color:#D3D3D3"
                  />
                </div>
                <div class="column justify-between q-gutter-y-sm">
                  <div class="text-subtitle2 text-weight-medium">
                    DATUM ROĐENJA: {{ playerInfo.birthDate | formatDate }}
                  </div>
                  <q-separator />
                  <div class="text-subtitle2 text-weight-medium">
                    VISINA: {{ playerInfo.height }} cm
                  </div>
                  <q-separator />
                  <div class="text-subtitle2 text-weight-medium">
                    TEŽINA: {{ playerInfo.weight }} kg
                  </div>
                  <q-separator />
                  <div class="text-subtitle2 text-weight-medium">
                    RUKA: {{ playerInfo.strongHand | capString }}
                  </div>
                  <q-separator />
                  <div class="text-subtitle2 text-weight-medium">
                    MARKA REKETA: {{ playerInfo.racketBrand | capString }}
                  </div>
                  <q-separator />
                  <div class="row">
                    <q-btn
                      flat
                      round
                      type="a"
                      color="green-3"
                      icon="fab fa-instagram"
                      :href="playerInfo.instagram"
                    />
                    <q-btn
                      flat
                      round
                      type="a"
                      color="primary"
                      icon="fab fa-twitter"
                      :href="playerInfo.twitter"
                    />
                  </div>
                </div>
              </q-card-section>
              <q-card-section class="q-pa-none">
                <div style="width:330px;height:440px;overflow:hidden">
                  <img :src="playerInfo.standingPicture" />
                </div>
              </q-card-section>
            </q-card-section>
          </q-card>
        </div>
        <q-card class="q-mx-xl shadow-2">
          <q-tabs
            class="dense bg-secondary text-white"
            v-model="tab"
            indicator-color="transparent"
            active-color="green-2"
            align="justify"
            inline-label
            @input="val => handleTabChange(val)"
          >
            <q-tab name="bio" icon="fas fa-info" label="Biografija" />
            <q-tab name="titles" icon="fas fa-medal" label="Titule" />
            <q-tab name="matches" icon="fas fa-running" label="Mečevi" />
            <q-tab name="ranks" icon="fas fa-chart-line" label="Rangiranje" />
          </q-tabs>
        </q-card>
        <q-tab-panels v-model="tab" animated class="q-mx-xl">
          <q-tab-panel name="bio">
            <div class="text-body2 text-justify">
              {{ playerInfo.bio }}
            </div>
          </q-tab-panel>

          <q-tab-panel name="titles">
            <matches-list :matches="titleMatches" title="Mečevi za titulu" />
          </q-tab-panel>

          <q-tab-panel name="matches">
            <matches-list :matches="recentMatches" />
          </q-tab-panel>

          <q-tab-panel name="ranks" class="column items-center">
            <q-select
              dense
              style="min-width:210px"
              outlined
              label="Godina"
              v-model="selectedRankYear"
              :options="rankYears"
              @input="val => handleSelectYearForRanking(val)"
            />
            <line-chart
              style="width:250px;height:400px"
              :chart-data="chartData"
            />
          </q-tab-panel>
        </q-tab-panels>
      </div>
    </div>
  </q-page>
</template>

<script>
import DoughnutChart from "src/components/app/DoughnutChart.vue";
import LineChart from "src/components/app/LineChart.vue";
import MatchesList from "src/components/app/MatchesList.vue";

import { globalAppMixin } from "src/mixins/globalAppMixin";

export default {
  components: { DoughnutChart, LineChart, MatchesList, MatchesList },
  name: "PlayerDetailsPage",
  mixins: [globalAppMixin],
  data() {
    return {
      playersList: [],
      selectedPlayer: null,
      tab: "bio",
      playerInfo: {},
      recentMatches: null,
      titleMatches: null,
      rankHistory: null,
      rankYears: [],
      selectedRankYear: null,
      chartData: null
    };
  },
  created() {
    this.$store
      .dispatch("apiRequest/getApiRequest", {
        url: `player/get_all_basic`
      })
      .then(response => {
        this.playersList = response.map(x => ({
          value: x.id,
          label: `${x.firstName} ${x.lastName}`
        }));
      });
  },
  methods: {
    handleGetPlayer() {
      this.recentMatches = null;
      this.finals = null;
      this.rankHistory = null;

      this.$store
        .dispatch("apiRequest/getApiRequest", {
          url: `player/get/${this.selectedPlayer.value}`
        })
        .then(response => {
          this.playerInfo = { ...response };
        });
    },
    handleTabChange(tab) {
      if (tab === "matches" && !this.recentMatches) {
        this.$store
          .dispatch("apiRequest/getApiRequest", {
            url: `player/get_recent_matches/${this.selectedPlayer.value}`
          })
          .then(response => {
            this.recentMatches = response;
          });
      } else if (tab === "titles" && !this.recentMatches) {
        this.$store
          .dispatch("apiRequest/getApiRequest", {
            url: `player/get_title_matches/${this.selectedPlayer.value}`
          })
          .then(response => {
            this.titleMatches = response;
          });
      } else if (tab === "ranks") {
        this.$store
          .dispatch("apiRequest/getApiRequest", {
            url: `rankings/get_ranks_for_player/${this.selectedPlayer.value}`
          })
          .then(response => {
            this.rankHistory = response;

            this.rankYears = Object.keys(this.rankHistory);
          });
      }
    },
    handleSelectYearForRanking(val) {
      this.chartData = {
        labels: this.rankHistory[val].map(x => x.weekDuration),
        datasets: [
          {
            fill: "start",
            data: this.rankHistory[val].map(x => x.rankNumber)
          }
        ]
      };
    }
  }
};
</script>
