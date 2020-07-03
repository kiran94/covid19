import VuexPersistence from 'vuex-persist'

export default ({ store }) => {
  window.onNuxtReady(() => {
    new VuexPersistence({
      key: 'covid19',
      storage: window.localStorage,
      modules: ['settings']
    }).plugin(store)
  })
}
