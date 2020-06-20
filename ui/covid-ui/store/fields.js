export const state = () => ({
  fields: []
})

export const mutations = {
  setFields: (state, fields) => (state.fields = fields)
}

export const actions = {

  async fetchFields({ commit }) {
    const data = await this.$axios.$post('/graphql',
    {
      query: `
        query {
          fields {
            iD,
            description
          }
        }
        `
    },
    {
      headers: {
        'Content-Type': 'application/json'
      }
    })

    commit('setFields', data['data']['fields'])
  }
}
