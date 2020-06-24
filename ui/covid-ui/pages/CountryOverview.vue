<template>
  <v-layout column>
    <v-flex x12 sm8 md6>
      <v-row align="center" justify="center">
        <v-col cols="12" md="4">
          <h1 class="display-1 text-center">Country Overview</h1>
        </v-col>
      </v-row>

      <v-row align="center" justify="center">
        <v-col cols="12" md="4">
          <SearchCountry
            v-model="request.selectedCountry"
            hideState
            hideCounty
            hideTitle
            hideSubtitle
          ></SearchCountry>

          <SearchDate
            v-if="false"
            v-model="request.selectedDate"
            hideTitle
          ></SearchDate>

          <v-btn
            depressed
            color="primary"
            @click="loadReportedDaily"
            block
            class="mt-2"
            ><v-icon>mdi-magnify</v-icon>Search</v-btn
          >
          <v-divider></v-divider>
        </v-col>
      </v-row>

      <div id="graphs">
        <v-row>
          <h2>Reported Daily</h2>
        </v-row>
        <v-row>
          <v-col cols="4">
            <h3>Deaths</h3>
            <LineChart
              :data="chartData.reportedDailyDeaths"
              :options="chartOptions"
            />
          </v-col>

          <v-col cols="4">
            <h3>Confirmed</h3>
            <LineChart
              :data="chartData.reportedDailyConfirmed"
              :options="chartOptions"
            />
          </v-col>

          <v-col cols="4">
            <h3>Recovered</h3>
            <LineChart
              :data="chartData.reportedDailyRecovered"
              :options="chartOptions"
            />
          </v-col>
        </v-row>
      </div>
    </v-flex>
  </v-layout>
</template>

<script>
import SearchCountry from '@/components/SearchCountry'
import SearchDate from '@/components/SearchDate'
import { LineChart } from '~/components/charts/charts.js'

export default {
  components: {
    SearchCountry,
    SearchDate,
    LineChart
  },
  head() {
    return {
      title: 'CountryOverview'
    }
  },
  data: function() {
    return {
      request: {
        selectedCountry: [],
        selectedDate: [],
        take: 500
      },
      timeseries: {
        reportedDailies: null
      },
      chartData: {
        reportedDailies: null,
        reportedDailyDeaths: null,
        reportedDailyConfirmed: null,
        reportedDailyRecovered: null
      },
      chartOptions: {
        maintainAspectRatio: false
      }
    }
  },
  methods: {
    async loadReportedDaily() {
      console.group('Loading Reported Daily Data')
      this.$nuxt.$loading.start()

      const fields = [
        'REPORTED_DAILY_CONFIRMED',
        'REPORTED_DAILY_DEATHS',
        'REPORTED_DAILY_RECOVERED'
      ]

      this.timeseries.reportedDailies = await this.loadData(fields)

      this.chartData.reportedDailyDeaths = this.generateGraph(
        this.timeseries.reportedDailies,
        'REPORTED_DAILY_DEATHS'
      )
      this.chartData.reportedDailyConfirmed = this.generateGraph(
        this.timeseries.reportedDailies,
        'REPORTED_DAILY_CONFIRMED'
      )
      this.chartData.reportedDailyRecovered = this.generateGraph(
        this.timeseries.reportedDailies,
        'REPORTED_DAILY_RECOVERED'
      )

      this.$toast.success('Loaded Country')
      this.$nuxt.$loading.finish()
      console.groupEnd()
    },

    generateGraph(data, field) {
      console.log(data)

      const records = data.filter((x) => x.field == field)
      const dates = records.map((x) => x.date)
      const values = records.map((x) => x.value)

      const chartData = {
        labels: dates,
        datasets: [
          {
            label: field,
            borderColor: '#41B38A',
            fill: false,
            data: values
          }
        ]
      }

      return chartData
    },

    async loadData(fields) {
      console.group('Requesting Timeseries Data')
      console.log(this.request)
      console.log(fields)

      const result = await this.$axios.$post('/graphql', {
        query: `
          query request(
            $country_region: [String],
            $province_state: [String],
            $counties: [String],
            $fields: [String],
            $take: Int) {

              timeseries(
                country_region: $country_region,
                province_state: $province_state,
                counties: $counties,
                fields: $fields,
                take: $take) {
                  date,
                  field,
                  value
                }
            }
        `,
        variables: {
          country_region: this.request.selectedCountry[0],
          province_state: '',
          counties: '',
          fields: fields,
          take: this.request.take
        }
      })

      console.log(result)
      console.groupEnd()
      return result['data']['timeseries']
    }
  }
}
</script>

<style></style>
