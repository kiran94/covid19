export const state = () => ({
  requests: [],
  currentRequest: null
})

export const mutations = {
  addRequest: (state, request) => {
    state.currentRequest = request
    state.requests.push(request)
  },
  clearRequests: (state) => {
    state.currentRequest = null
    state.requests = []
  }
}
