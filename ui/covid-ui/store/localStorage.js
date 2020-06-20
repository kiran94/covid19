export const state = () => ({
  useDarkMode: true
})

export const mutations = {
  setDarkMode: (state, newValue) => (state.useDarkMode = newValue)
}

export const getters = {
  getUseDarkMode: (state) => state.useDarkMode
}
