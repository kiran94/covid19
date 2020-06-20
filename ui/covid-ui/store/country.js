export const state = () => ({
  countries: []
})

export const mutations = {
  setCountries: (state, countries) => (state.countries = countries)
}

export const actions = {
  async fetchCountries({ commit }) {
    const data = await this.$axios.$post('/graphql', {
      query: `
      query {
          countries {
            countryRegion,
            provinceState,
            county
        }
      }
    `
    })

    commit('setCountries', data['data']['countries'])
  }
}

export const getters = {
  getCountries: (state) => state.countries
}
