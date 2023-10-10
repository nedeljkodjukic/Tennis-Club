<template>
  <q-dialog v-model="visible" persistent @hide="handleHide">
    <q-card class="q-py-sm full-width">
      <q-card-section class="row full-width justify-between items-center">
        <q-img src="~assets/logo2.png" width="60px" />
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
          @submit.prevent="handleAdd"
        >
          <div class="row justify-around">
            <q-img :src="formData.logo" width="50px" height="50px" />
            <q-file
              style="max-width: 240px"
              outlined
              dense
              v-model="picture"
              label="Izaberite sliku"
              @input="handleImageUpload"
            >
              <template v-slot:prepend>
                <q-icon name="attach_file" />
              </template>
            </q-file>
          </div>
          <div class="row justify-around">
            <q-input
              style="min-width: 240px"
              outlined
              dense
              placeholder="Ime"
              v-model="formData.name"
            />
            <q-select
              style="min-width: 240px"
              outlined
              dense
              label="Podloga"
              v-model="formData.surface"
              :options="surfaces"
            />
          </div>
          <div class="row justify-around">
            <q-input
              style="min-width: 240px"
              outlined
              dense
              placeholder="Grad"
              v-model="formData.city"
            />
            <q-select
              style="min-width: 240px"
              outlined
              dense
              label="Država"
              v-model="formData.country"
              :options="countries"
            />
          </div>
          <div class="row justify-around">
            <q-select
              style="min-width: 240px"
              outlined
              dense
              label="Kategorija"
              v-model="formData.category"
              :options="categories"
            />
            <q-input
              style="min-width: 240px"
              outlined
              dense
              label="Atp poena"
              v-model="formData.atpPoints"
              type="number"
            />
          </div>
          <q-input
            class="q-px-sm"
            outlined
            dense
            label="O turniru"
            v-model="formData.tournamentInfo"
            type="textarea"
          />
          <q-list>
            <q-item-label header class="row justify-between items-center">
              Tereni
              <q-btn flat round dense icon="add" @click="handleShowNewCourt" />
            </q-item-label>
            <q-item v-for="item in formData.courts" :key="item.name">
              <q-item-section avatar>
                <q-avatar rounded>
                  <img :src="item.picture" />
                </q-avatar>
              </q-item-section>
              <q-item-section> Ime: {{ item.name }} </q-item-section>
              <q-item-section> Kapacitet: {{ item.capacity }} </q-item-section>
              <q-item-section side>
                <q-btn
                  flat
                  dense
                  round
                  icon="delete"
                  @click="handleRemoveCourt(item.name)"
                />
              </q-item-section>
            </q-item>
            <q-separator />
            <q-item v-if="!newCourt.saved">
              <q-item-section>
                <q-file
                  outlined
                  dense
                  v-model="newCourt.picture"
                  label="Učitaj sliku"
                >
                  <template v-slot:prepend>
                    <q-icon name="attach_file" />
                  </template>
                </q-file>
              </q-item-section>
              <q-item-section>
                <q-input outlined dense v-model="newCourt.name" label="Ime" />
              </q-item-section>
              <q-item-section>
                <q-input
                  outlined
                  dense
                  v-model="newCourt.capacity"
                  label="Broj mesta"
                  type="number"
                />
              </q-item-section>
              <q-item-section side>
                <q-btn flat dense round icon="save" @click="handleAddCourt" />
              </q-item-section>
            </q-item>
          </q-list>
          <q-separator />
          <q-btn
            class="q-py-sm"
            type="submit"
            color="primary"
            label="Dodaj turnir"
            no-caps
            :loading="buttonLoading"
          />
        </q-form>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>

<script>
import { countries } from "assets/countries.js";

export default {
  name: "TournamentDialog",
  props: {
    visible: {
      type: Boolean,
      default: false
    },
    dataToEdit: {
      type: Object,
      required: false,
      default: null
    },
    toEdit: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      countries: [],
      surfaces: ["Trava", "Beton", "Šljaka"],
      categories: [
        "Grand Slam",
        "ATP Finals",
        "ATP Tour Masters 1000",
        "ATP Tour 500",
        "ATP Tour 250",
        "ATP Challenger Tour"
      ],
      newCourt: {
        name: null,
        capacity: null,
        saved: false
      },
      picture: null,
      formData: {
        name: null,
        city: null,
        country: null,
        surface: null,
        category: null,
        tournamentInfo: null,
        logo: null,
        atpPoints: 0,
        courts: []
      }
    };
  },
  mounted() {
    this.setCountries();
  },
  methods: {
    setCountries() {
      this.countries = countries.map(x => ({ value: x.alpha3, label: x.name }));
    },
    fileToBase64(file) {
      return new Promise(resolve => {
        var reader = new FileReader();
        reader.onload = function(event) {
          resolve(event.target.result);
        };
        reader.readAsDataURL(file);
      });
    },
    handleImageUpload(file) {
      debugger;
      this.fileToBase64(file).then(result => {
        this.formData.logo = result;
      });
    },
    handleHide() {
      this.formData = {};
      this.picture = null;
      this.$emit("hideDialog");
    },
    handleAddCourt() {
      debugger;
      this.fileToBase64(this.newCourt.picture).then(result => {
        this.formData.courts.push({
          picture: result,
          name: this.newCourt.name,
          capacity: this.newCourt.capacity
        });

        this.newCourt.saved = true;

        debugger;
      });
    },
    handleShowNewCourt() {
      if (this.newCourt.saved === true) {
        this.newCourt = {
          name: null,
          capacity: null,
          saved: false
        };
      }
    },
    handleAdd() {
      const payload = {
        ...this.formData,
        country: {
          name: this.formData.country.value,
          code: this.formData.country.label
        }
      };

      debugger;

      const url = this.toEdit
        ? "tournament/update/" + this.formData.id
        : "tournament/insert";

      this.$store
        .dispatch("apiRequest/postApiRequest", {
          url: url,
          data: payload,
          successMessage: "Uspešno ste dodali/ažurirali turnir"
        })
        .then(response => {
          this.handleHide();
        });
    },
    handleRemoveCourt(name) {
      debugger;
      const a = this.formData.courts.filter(x => x.name !== name);
      this.formData.courts = [
        ...this.formData.courts.filter(x => x.name !== name)
      ];
    }
  },
  computed: {
    buttonLoading() {
      return this.$store.state.apiRequest.isLoading;
    }
  },
  watch: {
    dataToEdit: function(old, newV) {
      debugger;
      if (this.dataToEdit) {
        this.formData = { ...this.dataToEdit };
      }
    }
  }
};
</script>

<style scoped></style>
