<template>
  <q-page>
    <q-form
      class="row q-py-md q-gutter-x-md full-width justify-center bg-secondary"
      @submit.prevent="handleSearch"
    >
      <q-select
        style="min-width:210px"
        dense
        outlined
        color="primary"
        bg-color="white"
        v-model="formData.year"
        :options="years"
        label="Godina"
        @input="val => handleWeekOptions(val)"
      />
      <q-select
        style="min-width:210px"
        dense
        outlined
        color="primary"
        bg-color="white"
        v-model="formData.week"
        :options="weekOptions"
        label="Nedelja"
      />
      <q-select
        style="min-width:210px"
        dense
        outlined
        color="primary"
        bg-color="white"
        v-model="formData.count"
        :options="playersCountOptions"
        label="Broj tenisera"
      />
      <q-btn
        label="Pretraži"
        color="green-2"
        type="submit"
        no-caps
        icon="search"
        style="min-width:140px"
        :disable="!(formData.count && formData.week && formData.year)"
      />
    </q-form>

    <div class="row">
      <div
        class="col-xs-12 col-sm-10 col-md-10 col-lg-8 offset-sm-1 offset-md-1 offset-lg-2"
      >
        <q-list class="rounded-borders full-width q-px-xl" separator>
          <q-item>
            <q-item-section>
              <div class="text-h5 text-weight-medium">RANG LISTA</div>
            </q-item-section>
            <q-item-section side>
              <q-select
                style="min-width:160px"
                dense
                borderless
                :options="sortingOptions"
                v-model="selectedSortingOption"
                label="Sortiraj po"
                @input="x => handleSort(x)"
              >
                <template v-slot:prepend>
                  <q-icon name="sort" />
                </template>
              </q-select>
            </q-item-section>
          </q-item>
          <q-separator inset />
          <div v-for="item in ranks" :key="item.playerId">
            <q-item
              class="q-gutter-x-lg"
              clickable
              @click="handlePlayerClick(item.playerId)"
            >
              <q-item-section style="min-width:80px;max-width:80px">
                <div class="row justify-end text-h5 text-weight-bolder">
                  {{ item.rank }}.
                </div>
              </q-item-section>
              <q-item-section avatar>
                <q-card>
                  <q-avatar rounded size="70px">
                    <img :src="item.picture" />
                  </q-avatar>
                </q-card>
              </q-item-section>
              <q-item-section>
                <div class="row">
                  <div class="text-h6 text-weight-regular q-px-sm">
                    {{ item.firstName }}
                  </div>
                  <div class="text-h6 text-weight-bold">
                    {{ item.lastName | capString }}
                  </div>
                </div>
              </q-item-section>
              <q-item-section>
                <div class="row q-gutter-x-sm items-center">
                  <q-avatar size="lg" square>
                    <q-img
                      style="border: 1px solid;border-color:#D3D3D3"
                      :src="getFlag(item.country.code)"
                    />
                  </q-avatar>
                  <div class="text-weight-medium text-subtitle1">
                    {{ item.country.name }}
                  </div>
                </div>
              </q-item-section>
              <q-item-section style="min-width:130px;max-width:130px">
                <div class="text-h6 text-weight-medium">
                  {{ item.points }}
                </div>
              </q-item-section>
              <q-item-section style="min-width:100px;max-width:100px">
                <div class="row q-gutter-sm justify-start items-center">
                  <q-icon
                    size="md"
                    :class="
                      item.previousRank - item.rank !== 0
                        ? item.previousRank - item.rank > 0
                          ? 'text-positive'
                          : 'text-negative'
                        : 'text-primary'
                    "
                    :name="
                      item.previousRank - item.rank !== 0
                        ? item.previousRank - item.rank > 0
                          ? 'fas fa-angle-double-up'
                          : 'fas fa-angle-double-down'
                        : 'fas fa-minus'
                    "
                  />
                  <div
                    v-if="item.previousRank - item.rank !== 0"
                    class="text-subtitle1 text-weight-medium"
                  >
                    {{ Math.abs(item.previousRank - item.rank) }}
                  </div>
                </div>
              </q-item-section>
            </q-item>
            <q-separator inset />
          </div>
        </q-list>
      </div>
    </div>
  </q-page>
</template>

<script>
import { globalAppMixin } from "src/mixins/globalAppMixin.js";

export default {
  name: "RankingPage",
  mixins: [globalAppMixin],
  data() {
    return {
      years: [],
      weeks: {},
      weekOptions: [],
      sortingOptions: [],
      selectedSortingOption: {
        value: 1,
        label: "Rang ▼"
      },
      playersCountOptions: [],
      formData: {
        year: null,
        week: null,
        count: null
      },
      ranks: []
    };
  },
  created() {
    this.$store
      .dispatch("apiRequest/getApiRequest", {
        url: `rankings/get_top_rated?count=10`
      })
      .then(response => {
        this.ranks = [...response];
      });

    this.$store
      .dispatch("apiRequest/getApiRequest", {
        url: `rankings/weeks?years=10`
      })
      .then(response => {
        this.years = Object.keys(response);
        this.weeks = response;
      });

    this.playersCountOptions = [10, 20, 50, 100];

    this.sortingOptions = [
      {
        value: 1,
        label: "Rang ▼"
      },
      {
        value: 2,
        label: "Rang ▲"
      },
      {
        value: 3,
        label: "Ime ▼"
      },
      {
        value: 4,
        label: "Ime ▲"
      },
      {
        value: 5,
        label: "Zemlja ▼"
      },
      {
        value: 6,
        label: "Zemlja ▲"
      }
    ];
  },
  methods: {
    handleWeekOptions(year) {
      const a = this.weeks[year];
      this.weekOptions = a.map(x => ({ value: x.weekNum, label: x.week }));
    },
    handlePlayerClick(id) {
      this.$router.push(`/player?id=${id}`);
    },
    handleSort(selected) {
      var arrClone = [...this.ranks];
      switch (selected.value) {
        case 1:
          this.ranks = [...arrClone.sort((a, b) => a.rank - b.rank)];
          break;
        case 2:
          this.ranks = [...arrClone.sort((a, b) => b.rank - a.rank)];
          break;
        case 3:
          this.ranks = arrClone.sort((a, b) =>
            a.firstName.localeCompare(b.firstName)
          );
          break;
        case 4:
          this.ranks = arrClone.sort((a, b) =>
            b.firstName.localeCompare(a.firstName)
          );
          break;
        case 5:
          this.ranks = arrClone.sort((a, b) =>
            a.country.name.localeCompare(b.country.name)
          );
          break;
        case 6:
          this.ranks = arrClone.sort((a, b) =>
            b.country.name.localeCompare(a.country.name)
          );
          break;
        default:
          this.ranks = arrClone;
      }
    },
    handleSearch() {
      this.$store
        .dispatch("apiRequest/getApiRequest", {
          url: `rankings/get_top_rated?year=${this.formData.year}&week=${this.formData.week.value}&count=${this.formData.count}`
        })
        .then(response => {
          this.ranks = [...response];
        });
    }
  }
};
</script>
