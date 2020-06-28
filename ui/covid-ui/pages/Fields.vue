<template>
  <v-layout column>
    <v-flex x12 sm8 md6>
      <v-row align="center" justify="center">
        <v-col cols="12" md="4">
          <h1 class="display-1 text-center">Fields</h1>
        </v-col>
      </v-row>

      <v-row align="center" justify="center">
        <v-col cols="12" md="8">
          <v-data-table :headers="headers" :items="items" items-per-page="50" >

            <template #item.color="{ item }">
              <span
                :style="{ backgroundColor: item.color, color: 'white' }"
                class="pa-1 rounded">
                {{ item.color }}
              </span>
            </template>

          </v-data-table>
        </v-col>
      </v-row>
    </v-flex>
  </v-layout>
</template>

<script>
export default {
  head() {
    return {
      title: 'Fields'
    }
  },
  data: function() {
    return {
      headers: [
        { text: 'ID', value: 'iD', align: 'start' },
        { text: 'Description', value: 'description' },
        { text: 'Color', value: 'color' }
      ]
    }
  },
  computed: {
    items: function() {
      return this.$store.state.fields.fields
    }
  }
}
</script>

<style>

</style>
