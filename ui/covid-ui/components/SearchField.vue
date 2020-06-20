<template>
  <div id="search_field">
    <v-card tile>
      <v-card-title class="headline">
        Search Field
      </v-card-title>

      <v-card-actions>
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
  props: ['value'],
  data: function() {
    return {
      selectedField: []
    }
  },
  computed: {
    fields: function() {
      return this.$store.state.fields.fields
    }
  },
  watch: {
    selectedField: function(val) {
      this.$emit('input', val)
    }
  }
}
</script>

<style></style>
