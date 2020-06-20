export const state = () => ({
  fields: []
})

export const mutations = {
  setFields: (state, fields) => (state.fields = fields)
}

export const actions = {
  async fetchFields({ commit }) {
    const data = await this.$axios.$post('/graphql', {
      query: `
        query {
          fields {
            iD,
            description
          }
        }
        `
    })

    commit('setFields', data['data']['fields'])
  }
}
