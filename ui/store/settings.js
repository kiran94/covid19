export const state = () => ({
  useDarkMode: null
})

export const mutations = {
  setDarkMode: (state, newValue) => (state.useDarkMode = newValue)
}
