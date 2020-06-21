<template>
  <div class="search_country">
    <v-card>
      <v-card-title class="headline" v-if="!hideTitle">
        Search Country
      </v-card-title>

      <v-card-actions v-if="!hideCountry">
        <v-card-subtitle v-if="!hideSubtitle">Country</v-card-subtitle>
        <v-autocomplete
          :items="countries"
          item-text="countryRegion"
          v-model="selectedCountry"
          autocomplete="new"
        />
      </v-card-actions>

      <v-card-actions v-if="!hideState">
        <v-card-subtitle v-if="!hideSubtitle">States</v-card-subtitle>
        <v-autocomplete
          :items="states"
          item-text="provinceState"
          v-model="selectedState"
          autocomplete="new"
        />
      </v-card-actions>

      <v-card-actions v-if="!hideCounty">
        <v-card-subtitle v-if="!hideSubtitle">County</v-card-subtitle>
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
  props: {
    value: Array,
    hideCountry: Boolean,
    hideState: Boolean,
    hideCounty: Boolean,
    hideTitle: Boolean,
    hideSubtitle: Boolean
  },
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
