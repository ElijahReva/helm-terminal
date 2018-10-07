import Vue from 'vue'
import Vuex from 'vuex'
import auth from './auth'
import nav from './navigation'
import manager from './manager'

import {state, getters, actions, mutations} from './rootStore'


import VuexPersistence  from 'vuex-persist'

import {SET_CONTEXT, SET_NAMESPACE} from "./rootConstants";

const vuexLocalRoot = new VuexPersistence({
    key: 'helm-terminal-local-root',
    storage: window.localStorage,
    filter: mutation => {
        return mutation.type === SET_NAMESPACE || mutation.type === SET_CONTEXT
    } 
});

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
    plugins: [vuexLocal.plugin, vuexLocalRoot.plugin]
})
