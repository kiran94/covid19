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
      this.$nuxt.$loading.start()

      const fields = [
        // Dailies
        this.$store.state.fields.fields.find(
          (x) => x.iD == 'REPORTED_DAILY_CONFIRMED'
        ),
        this.$store.state.fields.fields.find(
          (x) => x.iD == 'REPORTED_DAILY_DEATHS'
        ),
        this.$store.state.fields.fields.find(
          (x) => x.iD == 'REPORTED_DAILY_RECOVERED'
        ),

        // Totals
        this.$store.state.fields.fields.find(
          (x) => x.iD == 'REPORTED_TOTAL_CONFIRMED'
        ),
        this.$store.state.fields.fields.find(
          (x) => x.iD == 'REPORTED_TOTAL_DEATHS'
        ),
        this.$store.state.fields.fields.find(
          (x) => x.iD == 'REPORTED_TOTAL_RECOVERED'
        ),

        // Average Deaths
        this.$store.state.fields.fields.find(
          (x) => x.iD == 'ROLLING_AVERAGE_SEVENDAY_DEATHS'
        ),
        this.$store.state.fields.fields.find(
          (x) => x.iD == 'ROLLING_AVERAGE_FOURTEENDAY_DEATHS'
        ),
        this.$store.state.fields.fields.find(
          (x) => x.iD == 'ROLLING_AVERAGE_TWENTYONEDAY_DEATHS'
        ),

        // Averages Confirmed
        this.$store.state.fields.fields.find(
          (x) => x.iD == 'ROLLING_AVERAGE_SEVENDAY_CONFIRMED'
        ),
        this.$store.state.fields.fields.find(
          (x) => x.iD == 'ROLLING_AVERAGE_FOURTEENDAY_CONFIRMED'
        ),
        this.$store.state.fields.fields.find(
          (x) => x.iD == 'ROLLING_AVERAGE_TWENTYONEDAY_CONFIRMED'
        ),

        // Average Recovered
        this.$store.state.fields.fields.find(
          (x) => x.iD == 'ROLLING_AVERAGE_SEVENDAY_RECOVERED'
        ),
        this.$store.state.fields.fields.find(
          (x) => x.iD == 'ROLLING_AVERAGE_FOURTEENDAY_RECOVERED'
        ),
        this.$store.state.fields.fields.find(
          (x) => x.iD == 'ROLLING_AVERAGE_TWENTYONEDAY_RECOVERED'
        ),
      ]

      this.timeseries.reportedDailies = await this.loadData(
        fields.map((x) => x.iD)
      )

      this.chartData.reportedDaily.confirmed = this.generateGraph(
        this.timeseries.reportedDailies,
        fields[0]
      )
      this.chartData.reportedDaily.deaths = this.generateGraph(
        this.timeseries.reportedDailies,
        fields[1]
      )
      this.chartData.reportedDaily.recovered = this.generateGraph(
        this.timeseries.reportedDailies,
        fields[2]
      )

      this.chartData.reportedTotals.confirmed = this.generateGraph(
        this.timeseries.reportedDailies,
        fields[3]
      )
      this.chartData.reportedTotals.deaths = this.generateGraph(
        this.timeseries.reportedDailies,
        fields[4]
      )
      this.chartData.reportedTotals.recovered = this.generateGraph(
        this.timeseries.reportedDailies,
        fields[5]
      )

      this.chartData.rollingAverages.deaths = this.generateGraphMultiple(
        this.timeseries.reportedDailies,
        [fields[6], fields[7], fields[8]],
        this.chartData.rollingAverages.deaths
      )

      this.chartData.rollingAverages.confirmed = this.generateGraphMultiple(
        this.timeseries.reportedDailies,
        [fields[9], fields[10], fields[11]],
        this.chartData.rollingAverages.confirmed
      )

      this.chartData.rollingAverages.recovered = this.generateGraphMultiple(
        this.timeseries.reportedDailies,
        [fields[12], fields[13], fields[14]],
        this.chartData.rollingAverages.recovered
      )

      this.$toast.success('Loaded Country')
      this.$nuxt.$loading.finish()
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
