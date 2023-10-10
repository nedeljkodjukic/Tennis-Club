<template>
  <q-dialog v-model="visible" persistent @hide="handleHide">
    <q-card class="q-py-sm full-width">
      <q-card-section class="row full-width justify-between items-center">
        <div class="col-4"></div>
        <div class="col-4 text-center">
          <q-img src="~assets/logo2.png" width="60px" />
        </div>
        <div class="col-4   text-right">
          <q-btn
            icon="close"
            flat
            round
            dense
            @click="handleHide"
            color="primary"
          />
        </div>
      </q-card-section>
      <q-separator />
      <q-card-section>
        <q-form
          class="full-width column q-gutter-y-md q-pa-sm"
          @submit.prevent="handleAdd"
        >
          <div class="row justify-around">
            <q-img :src="formData.picture" width="240px" height="100px" />
            <q-img
              :src="formData.standingPicture"
              width="240px"
              height="100px"
            />
          </div>
          <div class="row justify-around items-end">
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
            <q-file
              style="max-width: 240px"
              outlined
              dense
              v-model="standingPicture"
              label="Izaberite sliku"
              @input="handleStandingImageUpload"
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
              v-model="formData.firstName"
            />
            <q-input
              style="min-width: 240px"
              outlined
              dense
              placeholder="Prezime"
              v-model="formData.lastName"
            />
          </div>
          <div class="row justify-around">
            <q-input
              style="min-width: 240px"
              outlined
              dense
              label="Datum rođenja"
              v-model="formData.birthDate"
            >
              <template v-slot:append>
                <q-icon name="event" class="cursor-pointer">
                  <q-popup-proxy
                    ref="qDateProxy"
                    transition-show="fade"
                    transition-hide="fade"
                  >
                    <q-date
                      v-model="formData.birthDate"
                      :mask="dateFormatString"
                      minimal
                      @input="() => $refs.qDateProxy.hide()"
                    />
                  </q-popup-proxy>
                </q-icon>
              </template>
            </q-input>
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
            <q-input
              style="min-width: 240px"
              outlined
              dense
              label="Visina"
              v-model="formData.height"
              type="number"
            />
            <q-input
              style="min-width: 240px"
              outlined
              dense
              label="Težina"
              v-model="formData.weight"
              type="number"
            />
          </div>
          <div class="row justify-around">
            <q-select
              style="min-width: 240px"
              outlined
              dense
              label="Ruka"
              v-model="formData.strongHand"
              :options="['Desna', 'Leva']"
            />
            <q-input
              style="min-width: 240px"
              outlined
              dense
              label="Marka reketa"
              v-model="formData.racketBrand"
            />
          </div>
          <div class="row justify-around">
            <q-input
              style="min-width: 240px"
              outlined
              dense
              label="Instagram"
              v-model="formData.instagram"
            />
            <q-input
              style="min-width: 240px"
              outlined
              dense
              label="Twitter"
              v-model="formData.twitter"
            />
          </div>
          <q-input
            outlined
            dense
            label="Biografija"
            v-model="formData.bio"
            type="textarea"
          />
          <q-separator />
          <q-btn
            class="q-py-sm"
            type="submit"
            color="primary"
            label="Dodaj igrača"
            no-caps
            :loading="buttonLoading"
          />
        </q-form>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>

<script>
import moment from "moment";

import { countries } from "assets/countries.js";

export default {
  name: "PlayerDialog",
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
      dateFormatString: "DD/MM/YYYY",
      countries: [],
      picture: null,
      standingPicture: null,
      formData: {
        firstName: null,
        lastName: null,
        birthDate: null,
        country: null,
        bio: null,
        strongHand: null,
        racketBrand: null,
        height: 0,
        weight: 0,
        picture: null,
        standingPicture: null,
        instagram: null,
        twitter: null
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
        this.formData.picture = result;
      });
    },
    handleStandingImageUpload(file) {
      debugger;
      this.fileToBase64(file).then(result => {
        this.formData.standingPicture = result;
      });
    },
    handleHide() {
      this.formData = {};
      this.picture = null;
      this.standingPicture = null;
      this.$emit("hideDialog");
    },
    handleAdd() {
      const payload = {
        ...this.formData,
        country: {
          name: this.formData.country.label,
          code: this.formData.country.value
        },
        height: parseInt(this.formData.height),
        weight: parseInt(this.formData.weight),
        accounts: [
          {
            socialNetwork: "Instagram",
            link: this.formData.instagram
          },
          {
            socialNetwork: "Twitter",
            link: this.formData.twitter
          }
        ],
        birthDate: moment(
          this.formData.birthDate,
          this.dateFormatString
        ).format()
      };

      debugger;

      const url = this.toEdit
        ? "player/update/" + this.formData.id
        : "player/insert";

      this.$store
        .dispatch("apiRequest/postApiRequest", {
          url: url,
          data: payload,
          successMessage: "Uspešno ste dodali/ažurirali igrača"
        })
        .then(response => {
          this.handleHide();
        });
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
