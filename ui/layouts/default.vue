<template>
  <v-app>
    <!-- MENU BAR -->
    <v-app-bar app flat class="primary" max-height="70">
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
      <v-spacer />
      <v-toolbar-title class="display-3 white--text">{{
        title
      }}</v-toolbar-title>
      <v-spacer />
      <template v-slot:extension>
        <v-tabs align-with-title class="mt-2 secondary">
          <v-tab class="white--text" to="/" nuxt>Home</v-tab>
          <v-tab class="white--text" to="/GlobalOverview" nuxt>
            <v-icon class="mr-1">mdi-earth</v-icon> Global Overview</v-tab
          >
          <v-tab class="white--text" to="/CountryOverview" nuxt
            ><v-icon class="mr-1">mdi-island</v-icon>Country Overview</v-tab
          >
          <v-tab class="white--text" to="/CountryComparison" nuxt
            ><v-icon class="mr-1">mdi-compare</v-icon>Country Comparison</v-tab
          >
          <v-tab class="white--text" to="/Fields" nuxt
            ><v-icon class="mr-1">mdi-table-arrow-left</v-icon>Fields</v-tab
          >
        </v-tabs>
      </template>
    </v-app-bar>

    <!-- NAVIGATION DRAWER -->
    <v-navigation-drawer v-model="drawer" width="500" fixed app>
      <v-expansion-panels multiple>
        <v-expansion-panel>
          <v-expansion-panel-header>Search</v-expansion-panel-header>
          <v-expansion-panel-content>
            <Search></Search>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-expansion-panels>
    </v-navigation-drawer>

    <!-- CONTENT -->
    <v-main>
      <v-container fluid>
        <nuxt keep-alive />
      </v-container>
    </v-main>

    <!-- FOOTER -->
    <v-footer app max-height="70">
      <span>&copy; {{ new Date().getFullYear() }}</span>
      <v-spacer></v-spacer>
      <v-switch v-model="dark" inset dense class="ma-1">
        <template v-slot:label>
          Dark Mode
        </template>
      </v-switch>
    </v-footer>
  </v-app>
</template>

<script>
import Search from '~/components/Search'

export default {
  components: {
    Search
  },
  data() {
    return {
      drawer: false,
      items: [
        {
          icon: 'mdi-apps',
          title: 'Welcome',
          to: '/'
        }
      ],
      miniVariant: false,
      title: 'COVID-19'
    }
  },
  computed: {
    dark: {
      get() {
        const val = this.$store.state.settings.useDarkMode
        // eslint-disable-next-line vue/no-side-effects-in-computed-properties
        this.$vuetify.theme.dark = val
        return val
      },
      set(newVal) {
        this.$store.commit('settings/setDarkMode', newVal)
      }
    }
  },
  created() {
    this.$store.dispatch('country/fetchCountries')
    this.$store.dispatch('fields/fetchFields')
  }
}
</script>
