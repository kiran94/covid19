<template>
  <div id="search_date">
    <v-card>
      <v-card-title class="headline">
        Select Date
      </v-card-title>

      <v-card-actions>
        <!-- <v-card-subtitle>Date</v-card-subtitle> -->
        <v-date-picker v-model="selectedDateRange" range header-color="primary" no-title />
      </v-card-actions>
    </v-card>
  </div>
</template>

<script>
export default {
  data: function() {
    return {
      selectedDateRange: []
    }
  }
}
</script>

<style></style>
