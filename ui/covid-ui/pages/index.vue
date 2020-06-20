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

        <v-col sm="12" md="3">
          <SearchCountry></SearchCountry>
        </v-col>

        <v-col sm="12" md="3">
          <SearchField></SearchField>
        </v-col>

        <v-col sm="12" md="3">
          <SearchDate></SearchDate>
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
  }
}
</script>
