<template>
  <div id="search_field">
    <v-card tile>
      <v-card-title class="headline">
        Search Field
      </v-card-title>

      <v-card-actions>
        <v-autocomplete
          v-model="selectedField"
          :items="fields"
          item-text="iD"
          autocomplete="new"
          color="primary"
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
  // eslint-disable-next-line vue/require-prop-types
  props: ['value'],
  data() {
    return {
      selectedField: []
    }
  },
  computed: {
    fields() {
      return this.$store.state.fields.fields
    }
  },
  watch: {
    selectedField(val) {
      this.$emit('input', val)
    }
  }
}
</script>

<style></style>
