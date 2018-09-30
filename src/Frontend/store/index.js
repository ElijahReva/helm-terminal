import Vue from 'vue'
import Vuex from 'vuex'
import auth from './auth'
import nav from './navigation'
import manager from './manager'
import { UPDATE_YAML } from './manager/actions'
import VuexPersistence  from 'vuex-persist'

// const vuexPersist = new VuexPersist({
//     key: 'helm-terminal',
//     storage: localStorage
// });

const vuexLocal = new VuexPersistence({
    key: 'helm-terminal-local',
    storage: window.localStorage,
    reducer: state => ({navigation: state.navigation}), //only save navigation module
    filter: mutation => (mutation.type === UPDATE_YAML)
});


Vue.use(Vuex);

export default new Vuex.Store({
    modules: {
        auth,
        nav,
        manager
    },
    plugins: [vuexLocal.plugin]
})
