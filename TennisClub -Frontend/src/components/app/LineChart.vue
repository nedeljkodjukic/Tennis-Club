<script>
import { Line, mixins } from "vue-chartjs";
const { reactiveProp } = mixins;

export default {
  name: "LineChart",
  extends: Line,
  mixins: [reactiveProp],
  data() {
    return {
      options: {}
    };
  },
  created() {
    this.options = {
      scales: {
        yAxes: [
          {
            ticks: {
              reverse: true,
              callback: function(value) {
                if (value % 1 === 0) {
                  return value;
                }
              }
            }
          }
        ]
      },
      legend: {
        display: false
      },
      tooltips: {
        enabled: false
      }
    };
  },
  mounted() {
    this.renderChart(this.chartData, this.options);
  }
};
</script>
