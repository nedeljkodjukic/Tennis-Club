<template>
  <div>
    <player-dialog
      :visible="playerDialogVisible"
      @hideDialog="handleHideDialog"
      :dataToEdit="dataToEdit"
      :toEdit="toEdit"
    />
    <q-table
      class="col-5 q-ma-xl"
      title="IGRAČI"
      :data="tableData"
      :columns="tableColumns"
      :filter="filter"
      row-key="name"
    >
      <template v-slot:top-right>
        <q-input dense debounce="300" v-model="filter" placeholder="Search">
          <template v-slot:append>
            <q-icon name="search" />
          </template>
        </q-input>

        <q-btn
          icon="add"
          flat
          round
          dense
          @click="handlePlayerAdd"
          color="primary"
        />
      </template>
      <template v-slot:body-cell-actions="props">
        <q-td :props="props">
          <div class="row justify-left q-gutter-md">
            <q-btn
              icon="edit"
              flat
              round
              dense
              @click="handlePlayerEdit(props.row.id)"
              color="primary"
            />

            <q-btn
              icon="delete"
              flat
              round
              dense
              @click="handlePlayerDelete(props.row.id)"
              color="primary"
            />
          </div>
        </q-td>
      </template>
    </q-table>
  </div>
</template>

<script>
import PlayerDialog from "src/components/admin/PlayerDialog.vue";
import { globalAppMixin } from "src/mixins/globalAppMixin";
export default {
  components: { PlayerDialog },
  name: "PlayersPage",
  mixins: [globalAppMixin],
  data() {
    return {
      playerDialogVisible: false,
      dataToEdit: null,
      toEdit: false,
      filter: null,
      tableColumns: [
        {
          name: "id",
          align: "left",
          label: "Id",
          field: "id",
          sortable: true
        },
        {
          name: "firstName",
          align: "left",
          label: "Ime",
          field: "firstName",
          sortable: true
        },
        {
          name: "lastName",
          align: "left",
          label: "Prezime",
          field: "lastName",
          sortable: true
        },
        {
          name: "birthDate",
          align: "left",
          label: "Datum rođenja",
          field: "birthDate",
          format: (val, row) => this.$options.filters.formatDate(val),
          sortable: true
        },
        {
          name: "country",
          align: "left",
          label: "Zemlja",
          field: row => row.country.name,
          sortable: true
        },
        {
          name: "height",
          align: "left",
          label: "Visina (cm)",
          field: "height",
          sortable: true
        },
        {
          name: "weight",
          align: "left",
          label: "Težina (kg)",
          field: "weight",
          sortable: true
        },
        {
          name: "racketBrand",
          align: "left",
          label: "Marka reketa",
          field: "racketBrand",
          sortable: true
        },
        {
          name: "strongHand",
          align: "left",
          label: "Ruka",
          field: "strongHand",
          sortable: true
        },
        {
          name: "actions",
          align: "left",
          label: "Akcije",
          field: "actions",
          sortable: false
        }
      ],
      tableData: []
    };
  },
  created() {
    this.$store
      .dispatch("apiRequest/getApiRequest", {
        url: "player/get_all"
      })
      .then(response => {
        this.tableData = response;
      });
  },
  methods: {
    handleHideDialog() {
      this.playerDialogVisible = false;
    },
    handlePlayerAdd() {
      this.toEdit = false;
      this.dataToEdit = null;
      this.playerDialogVisible = true;
    },
    handlePlayerEdit(value) {
      this.toEdit = true;
      const curr = this.tableData.filter(x => x.id === value)[0];
      this.dataToEdit = {
        ...curr,
        country: {
          value: curr.country.code,
          label: curr.country.name
        }
      };
      this.playerDialogVisible = true;
    },
    handlePlayerDelete(value) {
      this.$store
        .dispatch("apiRequest/deleteApiRequest", {
          url: "player/remove/" + value,
          successMessage: "Uspešno ste obrisali igrača"
        })
        .then(response => {});
    }
  }
};
</script>

<style scoped></style>
