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
            :disabled="isLoading"
            ><v-icon>mdi-magnify</v-icon>Search</v-btn
          >
          <v-divider></v-divider>
        </v-col>
      </v-row>

      <div id="graphs" v-if="timeseries.reportedDailies">
        <div id="reported_totals">
          <v-row>
            <h2>Reported Totals</h2>
          </v-row>
          <v-row>
            <v-col cols="4">
              <h3>Deaths</h3>
              <LineChart
                :data="chartData.reportedTotals.deaths"
                :options="chartOptions"
              />
            </v-col>

            <v-col cols="4">
              <h3>Confirmed</h3>
              <LineChart
                :data="chartData.reportedTotals.confirmed"
                :options="chartOptions"
              />
            </v-col>

            <v-col cols="4">
              <h3>Recovered</h3>
              <LineChart
                :data="chartData.reportedTotals.recovered"
                :options="chartOptions"
              />
            </v-col>
          </v-row>
        </div>

        <div id="reportedDailies">
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

        <div id="rollingAverages">
          <v-row><h2>Rolling Averages</h2></v-row>

          <v-row>
            <v-col cols="4">
              <h3>Deaths</h3>
              <LineChart
                :data="chartData.rollingAverages.deaths"
                :options="chartOptions"
              />
            </v-col>

            <v-col cols="4">
              <h3>Confirmed</h3>
              <LineChart
                :data="chartData.rollingAverages.confirmed"
                :options="chartOptions"
              />
            </v-col>

            <v-col cols="4">
              <h3>Recovered</h3>
              <LineChart
                :data="chartData.rollingAverages.recovered"
                :options="chartOptions"
              />
            </v-col>
          </v-row>
        </div>
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
      isLoading: false,
      request: {
        selectedCountry: [],
        selectedDate: [],
        take: 5000
      },
      timeseries: {
        reportedDailies: null
      },
      chartData: {
        reportedDaily: {
          deaths: null,
          confirmed: null,
          recovered: null
        },
        reportedTotals: {
          deaths: null,
          confirmed: null,
          recovered: null
        },
        rollingAverages: {
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
      this.isLoading = true
      this.$nuxt.$loading.start()

      const fieldList = [
        // Daily
        { name: 'REPORTED_DAILY_CONFIRMED', chartCallback: chartData => this.chartData.reportedDaily.confirmed = chartData },
        { name: 'REPORTED_DAILY_DEATHS', chartCallback: chartData => this.chartData.reportedDaily.deaths = chartData },
        { name: 'REPORTED_DAILY_RECOVERED', chartCallback: chartData => this.chartData.reportedDaily.recovered = chartData },

        // Daily Totals
        { name: 'REPORTED_TOTAL_CONFIRMED', chartCallback: chartData => this.chartData.reportedTotals.confirmed = chartData },
        { name: 'REPORTED_TOTAL_DEATHS', chartCallback: chartData => this.chartData.reportedTotals.deaths = chartData },
        { name: 'REPORTED_TOTAL_RECOVERED', chartCallback: chartData => this.chartData.reportedTotals.recovered = chartData },

        // Death Rolling Average
        {
          name: 'ROLLING_AVERAGE_DEATHS',
          multiple: true,
          subFields: ['ROLLING_AVERAGE_SEVENDAY_DEATHS', 'ROLLING_AVERAGE_FOURTEENDAY_DEATHS', 'ROLLING_AVERAGE_TWENTYONEDAY_DEATHS'],
          chartCallback: chartData => this.chartData.rollingAverages.deaths = chartData
        },
        // Confirmed Rolling Average
        {
          name: 'ROLLING_AVERAGE_CONFIRMED',
          multiple: true,
          subFields: ['ROLLING_AVERAGE_SEVENDAY_CONFIRMED', 'ROLLING_AVERAGE_FOURTEENDAY_CONFIRMED', 'ROLLING_AVERAGE_TWENTYONEDAY_CONFIRMED'],
          chartCallback: chartData => this.chartData.rollingAverages.confirmed = chartData
        },
        // Recovered Rolling Average
        {
          name: 'ROLLING_AVERAGE_RECOVERED',
          multiple: true,
          subFields: ['ROLLING_AVERAGE_SEVENDAY_RECOVERED', 'ROLLING_AVERAGE_FOURTEENDAY_RECOVERED', 'ROLLING_AVERAGE_TWENTYONEDAY_RECOVERED'],
          chartCallback: chartData => this.chartData.rollingAverages.recovered = chartData
        }
      ]

      console.log('Finding Fields to pull')
      let fields = []
      fieldList.forEach(field => {
        if (field.multiple) {
          field.subFields.forEach(element => {
            fields.push(this.$store.state.fields.fields.find(y => y.iD == element))
          });
        } else {
          fields.push(this.$store.state.fields.fields.find(y => y.iD == field.name))
        }
      });

      console.log('Requesting Timeseries Data')
      this.timeseries.reportedDailies = await this.loadData(fields.map((x) => x.iD))

      console.log('Generating Chart Data')
      for (let field in fieldList) {
        const currentField = fieldList[field]

        let chartData = null
        if (currentField.multiple) {
          chartData = this.generateGraphMultiple(this.timeseries.reportedDailies, fields.filter(x => currentField.subFields.includes(x.iD)), null, false)
        } else {
          chartData = this.generateGraph(this.timeseries.reportedDailies, fields.filter(x => x.iD == currentField.name)[0], null, false)
        }

        if (currentField.chartCallback) {
          currentField.chartCallback(chartData)
        }
      }

      this.$toast.success('Loaded Country')
      this.$nuxt.$loading.finish()
      this.isLoading = false
      console.groupEnd()
    },

    generateGraph(data, fieldInfo, chartData = null, fill = false) {
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

    generateGraphMultiple(data, fieldInfos, chartData = null, fill = false) {
      for (let i = 0; i < fieldInfos.length; i++) {
        if (i == 0) {
          chartData = this.generateGraph(data, fieldInfos[i], null, fill)
        } else {
          chartData = this.generateGraph(data, fieldInfos[i], chartData, fill)
        }
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
