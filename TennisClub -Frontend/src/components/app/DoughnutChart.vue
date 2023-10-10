<template>
  <div class="column items-center q-gutter-y-sm">
    <div :style="`height:${width}px; width:${width}px`">
      <base-doughnut-chart
        :chart-options="chartOptions"
        :chart-data="dataForChart"
      />
    </div>
    <div class="text-h5 text-weight-bold">{{ data[1] }} - {{ data[0] }}</div>
  </div>
</template>

<script>
import BaseDoughnutChart from "./BaseDoughnutChart.vue";

export default {
  name: "DoughnutChart",
  components: {
    BaseDoughnutChart
  },
  props: {
    title: {
      type: String,
      required: false,
      default: null
    },
    width: {
      type: Number,
      required: true
    },
    height: {
      type: Number,
      required: true
    },
    data: {
      type: Array
    },
    colors: {
      type: Array
    }
  },
  data() {
    return {
      dataForChart: null,
      chartOptions: null
    };
  },
  watch: {
    data: function() {
      this.setData();
    }
  },
  created() {
    this.setData();
    this.setOptions();
  },
  methods: {
    setData() {
      this.dataForChart = {
        labels: this.labels,
        datasets: [
          {
            backgroundColor: this.colors,
            data: this.data
          }
        ]
      };
    },
    setOptions() {
      this.chartOptions = {
        title: {
          display: this.title,
          text: this.title,
          fontSize: 14,
          fontColor: "#000"
        },
        legend: {
          display: false
        },
        tooltips: {
          enabled: false
        }
      };
    },
    drawChart() {
      if (this.dataForChart[0] === undefined) {
        this.dataForChart = [0, 0];
      }
      const context = document.getElementById(this.id);
      const chart = new Chart(context, {
        type: "doughnut",
        data: {
          labels: this.labels,
          datasets: [
            {
              backgroundColor: this.colors,
              data: this.dataForChart
            }
          ]
        },
        options: {
          title: {
            display: this.title,
            text: this.title,
            fontSize: 14,
            fontColor: "#000"
          },
          legend: {
            display: false
          },
          tooltips: {
            callbacks: {
              label: function(tooltipItems, data) {
                return dataForChart.labels[tooltipItems.index];
              }
            }
          }
        }
      });
    }
  }
};
</script>
