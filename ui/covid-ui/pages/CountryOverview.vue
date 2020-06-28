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
              :data="chartData.reportedDaily.deaths"
              :options="chartOptions"
            />
          </v-col>

          <v-col cols="4">
            <h3>Confirmed</h3>
            <LineChart
              :data="chartData.reportedDaily.confirmed"
              :options="chartOptions"
            />
          </v-col>

          <v-col cols="4">
            <h3>Recovered</h3>
            <LineChart
              :data="chartData.reportedDaily.recovered"
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
        reportedDaily: {
          deaths: null,
          confirmed: null,
          recovered: null
        }
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
        this.$store.state.fields.fields.find(x => x.iD == 'REPORTED_DAILY_CONFIRMED'),
        this.$store.state.fields.fields.find(x => x.iD == 'REPORTED_DAILY_DEATHS'),
        this.$store.state.fields.fields.find(x => x.iD == 'REPORTED_DAILY_RECOVERED'),
      ]

      this.timeseries.reportedDailies = await this.loadData(fields.map(x => x.iD))

      this.chartData.reportedDaily.confirmed = this.generateGraph(this.timeseries.reportedDailies, fields[0])
      this.chartData.reportedDaily.deaths = this.generateGraph(this.timeseries.reportedDailies, fields[1])
      this.chartData.reportedDaily.recovered = this.generateGraph(this.timeseries.reportedDailies, fields[2])

      this.$toast.success('Loaded Country')
      this.$nuxt.$loading.finish()
      console.groupEnd()
    },

    generateGraph(data, fieldInfo, chartData = null, fill = false) {
      console.debug(data)
      console.debug(fieldInfo)

      const records = data.filter((x) => x.field == fieldInfo.iD)
      const dates = records.map((x) => x.date)
      const values = records.map((x) => x.value)

      const newLine = {
        label: fieldInfo.description,
        borderColor: fieldInfo.color,
        fill: fill,
        data: values
      }

      if (chartData == null) {
        chartData = { labels: dates, datasets: [newLine] }
      } else {
        chartData.datasets.push(newLine)
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
          country_region: this.request.selectedCountry[0],
          province_state: '',
          counties: '',
          fields: fields,
          take: this.request.take,
          chronological: true
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
