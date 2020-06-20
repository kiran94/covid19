<template>
  <v-layout
    column
  >
    <v-flex
      xs12
      sm8
      md6
    >

      <v-row>

        <v-col sm="12" md="2">
          <SearchCountry></SearchCountry>
          <SearchField></SearchField>
          <SearchDate></SearchDate>
          <v-btn depressed color="primary" @click="search" block class="mt-2" >Search</v-btn>
        </v-col>

        <v-col sm="12" md="3">

        </v-col>

        <v-col sm="12" md="3">

        </v-col>

      </v-row>

    </v-flex>
  </v-layout>
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
  methods: {
    search() {
      console.log('Search Clicked')
    }
  }
}
</script>
