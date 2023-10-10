<script>
import { Doughnut, mixins } from "vue-chartjs";
const { reactiveProp } = mixins;

export default {
  name: "BaseDoughnutChart",
  extends: Doughnut,
  mixins: [reactiveProp],
  props: ["chartOptions"],
  mounted() {
    this.renderChart(this.chartData, this.chartOptions);
  }
};
</script>
