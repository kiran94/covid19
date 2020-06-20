<template>
  <div id="search_field">
    <v-card>
      <v-card-title class="headline">
        Search Field
      </v-card-title>

      <v-card-actions>
        <v-card-subtitle>Field</v-card-subtitle>

        <v-autocomplete
          :items="fields"
          item-text="iD"
          v-model="selectedField"
          autocomplete="no"
          chips
          deletable-chips
          multiple
        />
      </v-card-actions>
    </v-card>
  </div>
</template>

<script>
export default {
  data: function() {
    return {
      selectedField: []
    }
  },
  computed: {
    fields: function() {
      return this.$store.state.fields.fields
    }
  }
}
</script>

<style></style>
