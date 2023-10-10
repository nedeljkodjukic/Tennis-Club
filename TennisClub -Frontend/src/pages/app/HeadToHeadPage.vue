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
        v-model="firstPlayerId"
        :options="playersList"
        label="Igrač 1"
        @input="value => handleSearch()"
      />
      <div class="q-mx-md">
        <q-avatar color="accent" text-color="white" size="md">VS</q-avatar>
      </div>
      <q-select
        style="min-width:250px"
        dense
        outlined
        color="primary"
        bg-color="white"
        v-model="secondPlayerId"
        :options="playersList"
        label="Igrač 2"
        @input="value => handleSearch()"
      />
    </q-form>

    <div class="row" v-if="firstPlayer && secondPlayer">
      <div
        class="col-xs-12 col-sm-10 col-md-10 col-lg-8 offset-sm-1 offset-md-1 offset-lg-2"
      >
        <div class="row q-px-xl q-py-lg full-width justify-between">
          <div class="row col-5 items-end">
            <q-card>
              <q-img :src="firstPlayer.picture" width="150px" />
            </q-card>
            <div class="column justify-end q-px-md full-height">
              <div class="text-h5 text-weight-bold q-py-sm">
                #{{ firstPlayer.rank }}
              </div>
              <div class="text-h5 text-primary text-weight-medium">
                {{ firstPlayer.firstName }}
              </div>
              <div class="text-h4 q-pb-sm text-weight-bold text-primary">
                {{ firstPlayer.lastName | capString }}
              </div>
              <div class="row q-gutter-x-sm items-center">
                <q-avatar size="md" square>
                  <q-img
                    style="border: 1px solid;border-color:#D3D3D3"
                    :src="getFlag(firstPlayer.country.code)"
                  />
                </q-avatar>
                <div class="text-weight-bold">
                  {{ firstPlayer.country.name }}
                </div>
              </div>
            </div>
          </div>
          <div class="row col-2 justify-center items-center ">
            <doughnut-chart
              :colors="['#9aaebb', '#0094C6']"
              :data="[
                this.winsComparison.secondPlayerTotalWins,
                this.winsComparison.firstPlayerTotalWins
              ]"
              :main="true"
              :height="120"
              :width="120"
            />
          </div>
          <div class="row col-5 justify-end items-end">
            <div class="column justify-end q-px-md full-height items-end">
              <div class="text-h5 text-weight-bold q-py-sm">
                #{{ secondPlayer.rank }}
              </div>
              <div class="text-h5 text-secondary text-weight-medium">
                {{ secondPlayer.firstName }}
              </div>
              <div class="text-h4 q-pb-sm text-weight-bold text-secondary">
                {{ secondPlayer.lastName | capString }}
              </div>
              <div class="row q-gutter-x-sm items-center">
                <div class="text-weight-bold">
                  {{ secondPlayer.country.name }}
                </div>
                <q-avatar size="md" square>
                  <q-img
                    style="border: 1px solid;border-color:#D3D3D3"
                    :src="getFlag(secondPlayer.country.code)"
                  />
                </q-avatar>
              </div>
            </div>
            <q-card>
              <q-img :src="secondPlayer.picture" width="150px" />
            </q-card>
          </div>
        </div>
        <q-list padding class="rounded-borders full-width q-px-xl q-py-sm">
          <q-separator inset />
          <q-item>
            <q-item-section class="text-weight-bolder">{{
              firstPlayer.birthDate | formatDate
            }}</q-item-section>
            <q-item-section
              class="text-overline text-weight-bold q-pa-none text-center text-grey-7"
              >DATUM ROĐENJA</q-item-section
            >
            <q-item-section class="text-weight-bolder">
              <div class="row justify-end text-weight-bolder">
                {{ secondPlayer.birthDate | formatDate }}
              </div>
            </q-item-section>
          </q-item>
          <q-separator inset />
          <q-item>
            <q-item-section class="text-weight-bolder">
              {{ firstPlayer.height }} cm</q-item-section
            >
            <q-item-section
              class="text-overline text-weight-bold q-pa-none text-center text-grey-7"
              >VISINA
            </q-item-section>
            <q-item-section class="row justify-end">
              <div class="row justify-end text-weight-bolder">
                {{ secondPlayer.height }} cm
              </div>
            </q-item-section>
          </q-item>
          <q-separator inset />
          <q-item>
            <q-item-section class="text-weight-bolder">
              {{ firstPlayer.weight }} kg
            </q-item-section>
            <q-item-section
              class="text-overline text-weight-bold q-pa-none text-center text-grey-7"
              >TEŽINA
            </q-item-section>
            <q-item-section class="row justify-end">
              <div class="row justify-end text-weight-bolder">
                {{ secondPlayer.weight }} kg
              </div>
            </q-item-section>
          </q-item>
          <q-separator inset />
          <q-item>
            <q-item-section class="text-weight-bolder">
              {{ firstPlayer.strongHand }}</q-item-section
            >
            <q-item-section
              class="text-overline text-weight-bold q-pa-none text-center text-grey-7"
              >RUKA
            </q-item-section>
            <q-item-section class="row justify-end">
              <div class="row justify-end text-weight-bolder">
                {{ secondPlayer.strongHand }}
              </div>
            </q-item-section>
          </q-item>
          <q-separator inset />
          <q-item>
            <q-item-section class="text-weight-bolder">
              {{ firstPlayer.racketBrand }}</q-item-section
            >
            <q-item-section
              class="text-overline text-weight-bold q-pa-none text-center text-grey-7"
              >MARKA REKETA
            </q-item-section>
            <q-item-section class="row justify-end">
              <div class="row justify-end text-weight-bolder">
                {{ secondPlayer.racketBrand }}
              </div>
            </q-item-section>
          </q-item>
          <q-separator inset />
          <q-item>
            <q-item-section class="text-weight-bolder">
              {{ firstPlayer.titles }}</q-item-section
            >
            <q-item-section
              class="text-overline text-weight-bold q-pa-none text-center text-grey-7"
              >BROJ TITULA
            </q-item-section>
            <q-item-section class="row justify-end">
              <div class="row justify-end text-weight-bolder">
                {{ secondPlayer.titles }}
              </div>
            </q-item-section>
          </q-item>
          <q-separator inset />
        </q-list>
        <div class="row full-width q-px-xl q-py-md">
          <div class="col-6">
            <matches-list :matches="winsComparison.recentMatches" />
          </div>
          <div class="column col-6">
            <div class="row justify-around">
              <doughnut-chart
                :height="160"
                :width="160"
                :colors="['#9aaebb', '#0094C6']"
                :data="[
                  this.winsComparison.secondPlayerWins['Trava'],
                  this.winsComparison.firstPlayerWins['Trava']
                ]"
                title="Trava"
              />
              <doughnut-chart
                :height="160"
                :width="160"
                :colors="['#9aaebb', '#0094C6']"
                :data="[
                  this.winsComparison.secondPlayerWins['Šljaka'],
                  this.winsComparison.firstPlayerWins['Šljaka']
                ]"
                title="Šljaka"
              />
            </div>
            <div class="row justify-around">
              <doughnut-chart
                :height="160"
                :width="160"
                :colors="['#9aaebb', '#0094C6']"
                :data="[
                  this.winsComparison.secondPlayerWins['Beton'],
                  this.winsComparison.firstPlayerWins['Beton']
                ]"
                title="Beton"
              />
              <doughnut-chart
                :height="160"
                :width="160"
                :colors="['#9aaebb', '#0094C6']"
                :data="[
                  this.winsComparison.secondPlayerGrandSlamWins,
                  this.winsComparison.firstPlayerGrandSlamWins
                ]"
                title="Grand slam"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  </q-page>
</template>

<script>
import DoughnutChart from "src/components/app/DoughnutChart.vue";
import MatchesList from "src/components/app/MatchesList.vue";

import { globalAppMixin } from "src/mixins/globalAppMixin.js";

export default {
  components: { DoughnutChart, MatchesList },
  name: "HeadToHeadPage",
  mixins: [globalAppMixin],
  data() {
    return {
      playersList: [],
      firstPlayerId: null,
      secondPlayerId: null,
      firstPlayer: null,
      secondPlayer: null,
      winsComparison: {}
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
        const first = response[0].id;
        const second = response[1].id;
        this.handleSearch(first, second);
      });
  },
  methods: {
    handleSearch(first = null, second = null) {
      if (first === null && second === null) {
        first = this.firstPlayerId?.value;
        second = this.secondPlayerId?.value;
      }

      if (!first || !second) return;

      this.$store
        .dispatch("apiRequest/getApiRequest", {
          url: `player/get/${first}`
        })
        .then(response => {
          this.firstPlayer = response;
        });
      this.$store
        .dispatch("apiRequest/getApiRequest", {
          url: `player/get/${second}`
        })
        .then(response => {
          this.secondPlayer = response;
        });
      this.$store
        .dispatch("apiRequest/getApiRequest", {
          url: `player/get_head_to_head?firstPlayerId=${first}&secondPlayerId=${second}`
        })
        .then(response => {
          this.winsComparison = response;
        });
    },
    handlePlayerClick(id) {
      this.$router.push(`/player/${id}`);
    }
  }
};
</script>
