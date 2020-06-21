<template>
  <div>
    <v-list>
      <v-list-tile>
        <SearchCountry v-model="searchedCountry"></SearchCountry>
      </v-list-tile>

      <v-list-tile>
        <SearchField v-model="searchedFields"></SearchField>
      </v-list-tile>

      <v-list-tile>
        <SearchDate v-model="selectedDates"></SearchDate>
      </v-list-tile>

      <v-list-tile>
        <v-btn depressed color="primary" @click="search" block class="mt-2"
          ><v-icon>mdi-magnify</v-icon>Search</v-btn
        >
      </v-list-tile>
    </v-list>
  </div>
</template>

<script>
import SearchCountry from '~/components/SearchCountry'
import SearchField from '~/components/SearchField'
import SearchDate from '~/components/SearchDate'

export default {
  components: {
    SearchCountry,
    SearchField,
    SearchDate
  },
  data() {
    return {
      searchedCountry: [],
      searchedFields: [],
      selectedDates: []
    }
  },
  methods: {
    search() {
      console.group('Submitted Search')

      const request = {
        country: this.searchedCountry,
        fields: this.searchedFields,
        dates: this.selectedDates
      }

      console.log(request)

      this.$store.commit('selected/addRequest', request)
      console.groupEnd()
    }
  }
}
</script>

<style></style>
