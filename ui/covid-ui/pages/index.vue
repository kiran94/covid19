<template>
  <div class="bar-chart">
    <BarChart :data="barChartData" :options="{ maintainAspectRatio: false }" />
    <LineChart
      :data="lineChartData"
      :options="{ maintainAspectRatio: false }"
    />
  </div>
</template>

<script>
import moment from 'moment'
import { BarChart, LineChart } from '~/components/charts/charts.js'
export default {
  components: {
    BarChart,
    LineChart
  },
  data: function() {
    return {
      barChartData: {
        labels: ['Hello', 'World', 'Voltorb'],
        datasets: [
          {
            label: 'Nuxt.js Commit Activity',
            backgroundColor: '#41B38A',
            data: ['60', '20', '30']
          },
          {
            label: 'Nuxt.js Commit Activity',
            backgroundColor: '#41B38A',
            data: ['30', '40', '50']
          }
        ]
      },
      lineChartData: {
        labels: ['First', 'Second', 'Third'],
        datasets: [
          {
            label: 'Data One',
            borderColor: '#f87979',
            fill: false,
            data: ['60', '20', '30']
          },
          {
            label: 'Data Two',
            borderColor: '#41B38A',
            fill: false,
            data: ['30', '40', '50']
          }
        ]
      }
    }
  }
}
</script>

<style scoped>
.bar-chart {
  width: 80%;
  height: 80%;
  margin: auto;
  margin-top: 30px;
}
</style>
