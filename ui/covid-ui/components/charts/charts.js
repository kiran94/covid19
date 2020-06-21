
import { Bar, Line } from 'vue-chartjs'

export const BarChart = {
  extends: Bar,
  props: ['data', 'options'],
  mounted () {
    this.renderChart(this.data, this.options)
  }
}

export const LineChart = {
  extends: Line,
  props: ['data', 'options'],
  mounted() {
    this.renderLineChart()
  },
  methods: {
    renderLineChart: function() {
      this.renderChart(this.chartData, this.options)
    }
  },
  watch: {
    data: function() {
      // this._chart.destroy()
      this.renderLineChart()
    }
  },
  computed: {
    chartData: function() {
      return this.data
    }
  }
}
