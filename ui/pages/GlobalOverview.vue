<template>
  <v-layout column>
    <v-flex x12 sm8 md6>
      <v-row align="center" justify="center">
        <v-col cols="12" md="4">
          <h1 class="display-1 text-center">Global Overview</h1>
        </v-col>
      </v-row>

      <v-row>
        <v-col cols="12" md="4">
          <v-data-table
            :headers="topCountries.headers"
            :items="topCountries.data"
            hide-default-footer
          >
            <template v-slot:item.countryRegion="{ item }">
              <span> {{ item.countryRegion }} </span>
            </template>

            <template v-slot:item.value="{ item }">
              <span> {{ item.value }} </span>
            </template>
          </v-data-table>
        </v-col>

        <v-col cols="12" md="8">
          <LineChart
            :data="chartData.reportedDaily.deaths"
            :options="chartOptions"
          />
        </v-col>
      </v-row>

      <v-row>
        <v-col cols="12" md="4"></v-col>

        <v-col cols="12" md="8">
          <LineChart
            :data="chartData.reportedDaily.confirmed"
            :options="chartOptions"
          />
        </v-col>
      </v-row>
    </v-flex>
  </v-layout>
</template>

<script>
import { LineChart } from '~/components/charts/charts.js'

export default {
  components: {
    LineChart
  },
  data() {
    return {
      chartData: {
        reportedDaily: {
          deaths: null,
          confirmed: null
        }
      },
      chartOptions: {
        maintainAspectRatio: false
      },
      topCountries: {
        data: [],
        headers: [
          {
            text: 'Country / Region',
            align: 'start',
            value: 'countryRegion',
            sortable: false
          },
          { text: 'Value', align: 'start', value: 'value', sortable: false }
        ]
      }
    }
  },
  async mounted() {
    const results = await this.loadData([
      'REPORTED_DAILY_DEATHS',
      'REPORTED_DAILY_CONFIRMED'
    ])

    const reportedDailyDeaths = results.filter(
      (x) => x.field === 'REPORTED_DAILY_DEATHS'
    )

    const dates = reportedDailyDeaths.map((x) => x.date)
    const chartData = {
      labels: dates,
      datasets: [
        {
          label: 'REPORTED_DAILY_DEATHS',
          borderColor: '#FFF',
          fill: true,
          data: reportedDailyDeaths.map((x) => x.value)
        }
      ]
    }

    this.chartData.reportedDaily.deaths = chartData

    /// /

    const reportedDailyConfirmed = results.filter(
      (x) => x.field === 'REPORTED_DAILY_CONFIRMED'
    )

    const dates2 = reportedDailyConfirmed.map((x) => x.date)
    const chartData2 = {
      labels: dates2,
      datasets: [
        {
          label: 'REPORTED_DAILY_CONFIRMED',
          borderColor: '#FFF',
          fill: true,
          data: reportedDailyConfirmed.map((x) => x.value)
        }
      ]
    }

    this.chartData.reportedDaily.confirmed = chartData2

    this.topCountries.data = await this.getTopCountries([
      'REPORTED_DAILY_DEATHS'
    ])
  },
  methods: {
    async loadData(fields) {
      console.group('Requesting Timeseries Data')
      console.log(fields)

      const result = await this.$axios.$post('/graphql', {
        query: `
          query request(
            $country_region: [String],
            $province_state: [String],
            $counties: [String],
            $fields: [String],
            $take: Int,
            $chronological: Boolean) {

              timeseries(
                country_region: $country_region,
                province_state: $province_state,
                counties: $counties,
                fields: $fields,
                take: $take,
                chronological: $chronological) {
                  date,
                  field,
                  value
                }
            }
        `,
        variables: {
          country_region: 'Earth',
          province_state: '',
          counties: '',
          fields,
          take: 1000,
          chronological: true
        }
      })

      console.log(result)
      console.groupEnd()
      return result.data.timeseries
    },
    async getTopCountries(fields) {
      console.group('Requesting Timeseries Data')
      console.log(fields)

      const result = await this.$axios.$post('/graphql', {
        query: `
          query request(
            $province_state: [String],
            $counties: [String],
            $fields: [String],
            $dates: [DateTime]
            $take: Int,
            $order_value_descending: Boolean) {

              timeseries(
                province_state: $province_state,
                counties: $counties,
                fields: $fields,
                dates: $dates,
                take: $take,
                order_value_descending: $order_value_descending) {
                  countryRegion,
                  value
                }
            }
        `,
        variables: {
          province_state: '',
          counties: '',
          fields,
          dates: '2020-07-03', // TODO: Needs to be relative when we have date service
          take: 10,
          order_value_descending: true
        }
      })

      console.log(result)
      console.groupEnd()
      return result.data.timeseries
    }
  },
  head() {
    return {
      title: 'Global Overview'
    }
  }
}
</script>

<style></style>
