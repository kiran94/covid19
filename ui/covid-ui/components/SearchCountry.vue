<template>
  <div class="search_country">
    <v-card>
      <v-card-title v-if="!hideTitle" class="headline">
        Search Country
      </v-card-title>

      <v-card-actions v-if="!hideCountry">
        <v-card-subtitle v-if="!hideSubtitle">Country</v-card-subtitle>
        <v-autocomplete
          v-model="selectedCountry"
          :items="countries"
          item-text="countryRegion"
          autocomplete="new"
        />
      </v-card-actions>

      <v-card-actions v-if="!hideState">
        <v-card-subtitle v-if="!hideSubtitle">States</v-card-subtitle>
        <v-autocomplete
          v-model="selectedState"
          :items="states"
          item-text="provinceState"
          autocomplete="new"
        />
      </v-card-actions>

      <v-card-actions v-if="!hideCounty">
        <v-card-subtitle v-if="!hideSubtitle">County</v-card-subtitle>
        <v-autocomplete
          v-model="selectedCounty"
          :items="counties"
          item-text="county"
          autocomplete="new"
        />
      </v-card-actions>
    </v-card>
  </div>
</template>

<script>
export default {
  props: {
    // eslint-disable-next-line vue/require-default-prop
    value: Array,
    hideCountry: Boolean,
    hideState: Boolean,
    hideCounty: Boolean,
    hideTitle: Boolean,
    hideSubtitle: Boolean
  },
  data() {
    return {
      selectedCountry: null,
      selectedState: null,
      selectedCounty: null
    }
  },
  computed: {
    countries() {
      return this.$store.state.country.countries
    },
    states() {
      return this.countries.filter(
        (x) => x.countryRegion === this.selectedCountry
      )
    },
    counties() {
      return this.countries.filter(
        (x) =>
          x.countryRegion === this.selectedCountry &&
          x.provinceState === this.selectedState
      )
    }
  },
  watch: {
    selectedCountry() {
      this.$emit('input', [
        this.selectedCountry,
        this.selectedState,
        this.selectedCounty
      ])
    },
    selectedState() {
      this.$emit('input', [
        this.selectedCountry,
        this.selectedState,
        this.selectedCounty
      ])
    },
    selectedCounty() {
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
