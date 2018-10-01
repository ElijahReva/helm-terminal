import Vue from 'vue'
import Vuex from 'vuex'
import auth from './auth'
import nav from './navigation'
import manager from './manager'

import {state, getters, actions, mutations} from './rootStore'


import VuexPersistence  from 'vuex-persist'

// const vuexPersist = new VuexPersist({
//     key: 'helm-terminal',
//     storage: localStorage
// });

const vuexLocal = new VuexPersistence({
    key: 'helm-terminal-local',
    modules: [
        'manager',
        'nav'
    ],
    storage: window.localStorage,    
});


Vue.use(Vuex);

export default new Vuex.Store({
    state,
    getters,
    actions,
    mutations,
    modules: {
        auth,
        nav,
        manager,
    },
    plugins: [vuexLocal.plugin]
})
