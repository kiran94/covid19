<template>
  <div class="search_country">
    <v-card>
      <v-card-title class="headline">
        Search Country
      </v-card-title>

      <v-card-actions>
        <v-card-subtitle>Country</v-card-subtitle>
        <v-autocomplete
          :items="countries"
          item-text="countryRegion"
          v-model="selectedCountry"
          autocomplete="new"
        />
      </v-card-actions>

      <v-card-actions>
        <v-card-subtitle>States</v-card-subtitle>
        <v-autocomplete
          :items="states"
          item-text="provinceState"
          v-model="selectedState"
          autocomplete="new"
        />
      </v-card-actions>

      <v-card-actions>
        <v-card-subtitle>County</v-card-subtitle>
        <v-autocomplete
          :items="counties"
          item-text="county"
          v-model="selectedCounty"
          autocomplete="new"
        />
      </v-card-actions>
    </v-card>
  </div>
</template>

<script>
export default {
  props: ['value'],
  data: function() {
    return {
      selectedCountry: null,
      selectedState: null,
      selectedCounty: null
    }
  },
  computed: {
    countries: function() {
      return this.$store.state.country.countries
    },
    states: function() {
      return this.countries.filter(
        (x) => x.countryRegion == this.selectedCountry
      )
    },
    counties: function() {
      return this.countries.filter(
        (x) =>
          x.countryRegion == this.selectedCountry &&
          x.provinceState == this.selectedState
      )
    }
  },
  watch: {
    selectedCountry: function(val) {
      this.$emit('input', [
        this.selectedCountry,
        this.selectedState,
        this.selectedCounty
      ])
    },
    selectedState: function(val) {
      this.$emit('input', [
        this.selectedCountry,
        this.selectedState,
        this.selectedCounty
      ])
    },
    selectedCounty: function(val) {
      this.$emit('input', [
        this.selectedCountry,
        this.selectedState,
        this.selectedCounty
      ])
    }
  }
}
</script>

<style></style>
