import { } from './actions'
import {NAV_GOTO} from "./actions";

export const state = {
    tabs:[
        "manager",
        "watcher",
        "logs",
    ]
};

export const getters = {
    tabs: (state, commit, rootState ) => {
        return state.tabs.map(tab  => {
            return {
                path: '/' + tab,
                title: tab.replace(/^\w/, c => c.toUpperCase()),
                isActive: rootState.route.path.endsWith(tab)
            }
        })
    },
};

export const mutations = {
    [NAV_GOTO] : (state, path) => {
        state.currentRoute = path;
    },
};

export const actions = {
    [NAV_GOTO]: ({ commit, state }, path) => {
        commit(NAV_GOTO, path);
    }
};
