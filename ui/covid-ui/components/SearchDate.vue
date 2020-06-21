<template>
  <div id="search_date">
    <v-card tile>
      <v-card-title class="headline" v-if="!hideTitle">
        Select Dates
      </v-card-title>

      <v-card-actions class="justify-center">
        <v-date-picker
          v-model="selectedDateRange"
          range
          color="primary"
          no-title
          full-width
        />
      </v-card-actions>
    </v-card>
  </div>
</template>

<script>
export default {
  props: {
    value: Array,
    hideTitle: Boolean
  },
  data: function() {
    return {
      selectedDateRange: []
    }
  },
  watch: {
    selectedDateRange: function(val) {
      this.$emit('input', val)
    }
  }
}
</script>

<style></style>
