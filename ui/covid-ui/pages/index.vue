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
          <!-- <SearchCountry></SearchCountry> -->
        </v-col>

        <v-col sm="12" md="3">
          <!-- <SearchCountry></SearchCountry> -->
        </v-col>

      </v-row>

    </v-flex>
  </v-layout>
</template>

<script>
import SearchCountry from '~/components/SearchCountry'

export default {
  components: {
    SearchCountry
  }
}
</script>
