import Vue from 'vue'
import Vuex from 'vuex'
import auth from './auth'
import nav from './navigation'
import manager from './manager'
import signalr from './signalr'
import VuexPersistence  from 'vuex-persist'

// const vuexPersist = new VuexPersist({
//     key: 'helm-terminal',
//     storage: localStorage
// });

const vuexLocal = new VuexPersistence({
    key: 'helm-terminal-local',
    modules: [
        'manager'
    ],
    storage: window.localStorage,    
});


Vue.use(Vuex);

export default new Vuex.Store({
    modules: {
        auth,
        nav,
        manager,
        signalr,
    },
    plugins: [vuexLocal.plugin]
})
