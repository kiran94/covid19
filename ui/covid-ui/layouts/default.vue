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
    </v-app-bar>

    <v-navigation-drawer
      v-model="drawer"
      width="500"
      fixed
      app>

      <Search></Search>

    </v-navigation-drawer>

    <!-- CONTENT -->
    <v-main>
      <v-container fluid>
        <nuxt />
      </v-container>
    </v-main>

    <!-- FOOTER -->
    <v-footer app max-height="70">
      <span>&copy; {{ new Date().getFullYear() }}</span>
      <v-spacer></v-spacer>
      <v-switch inset dense v-model="dark" class="ma-1">
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
  created: function() {
    this.$store.dispatch('country/fetchCountries')
    this.$store.dispatch('fields/fetchFields')

    const theme = this.$store.state.localStorage.useDarkMode

    if (theme) {
      this.$vuetify.theme.dark = true
    } else {
      this.$vuetify.theme.dark = false
    }

    this.dark = this.$vuetify.theme.dark
  },
  data() {
    return {
      dark: false,
      drawer: true,
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
  watch: {
    dark: function(val) {
      this.$vuetify.theme.dark = val
      this.$store.commit('localStorage/setDarkMode', val)
    }
  }
}
</script>
