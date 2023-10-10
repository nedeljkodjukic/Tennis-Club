<template>
  <div>
    <tournament-dialog
      :visible="tournamentDialogVisible"
      @hideDialog="handleHideDialog"
      :dataToEdit="dataToEdit"
      :toEdit="toEdit"
    />
    <q-table
      class="q-ma-xl"
      title="TURNIRI"
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
          @click="handleTournamentAdd"
          color="primary"
        />
      </template>
      <template v-slot:body-cell-actions="props">
        <q-td :props="props">
          <div class="row justify-left q-gutter-md  ">
            <q-btn
              icon="edit"
              flat
              round
              dense
              @click="handleTournamentEdit(props.row.id)"
              color="primary"
            />

            <q-btn
              icon="delete"
              flat
              round
              dense
              @click="handleTournamentDelete(props.row.id)"
              color="primary"
            />
          </div>
        </q-td>
      </template>
    </q-table>
  </div>
</template>

<script>
import TournamentDialog from "src/components/admin/TournamentDialog.vue";
export default {
  components: { TournamentDialog },
  name: "TournamentsPage",
  data() {
    return {
      tournamentDialogVisible: false,
      toEdit: false,
      dataToEdit: null,
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
          name: "name",
          align: "left",
          label: "Ime",
          field: "name",
          sortable: true
        },
        {
          name: "city",
          align: "left",
          label: "Grad",
          field: "city",
          sortable: true
        },
        {
          name: "country",
          align: "left",
          label: "Država",
          field: row => row.country.name,
          sortable: true
        },
        {
          name: "surface",
          align: "left",
          label: "Podloga",
          field: "surface",
          sortable: true
        },
        {
          name: "category",
          align: "left",
          label: "Kategorija",
          field: "category",
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
        url: "tournament/get_all"
      })
      .then(response => {
        debugger;
        this.tableData = response;
      });
  },
  methods: {
    handleHideDialog() {
      this.tournamentDialogVisible = false;
    },
    handleTournamentAdd() {
      this.toEdit = false;
      this.dataToEdit = null;
      this.tournamentDialogVisible = true;
    },
    handleTournamentEdit(value) {
      this.toEdit = true;
      const curr = this.tableData.filter(x => x.id === value)[0];
      this.dataToEdit = {
        ...curr,
        country: {
          value: curr.country.code,
          label: curr.country.name
        }
      };
      this.tournamentDialogVisible = true;
    },
    handleTournamentDelete(value) {
      this.$store
        .dispatch("apiRequest/deleteApiRequest", {
          url: "tournament/remove/" + value,
          successMessage: "Uspešno ste obrisali turnir"
        })
        .then(response => {});
    }
  }
};
</script>

<style scoped></style>
