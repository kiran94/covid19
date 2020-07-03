<template>
  <div id="search_date">
    <v-card tile>
      <v-card-title v-if="!hideTitle" class="headline">
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
    // eslint-disable-next-line vue/require-default-prop
    value: Array,
    hideTitle: Boolean
  },
  data() {
    return {
      selectedDateRange: []
    }
  },
  watch: {
    selectedDateRange(val) {
      this.$emit('input', val)
    }
  }
}
</script>

<style></style>
