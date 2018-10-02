import {HubConnectionBuilder, LogLevel} from '@aspnet/signalr'
import {INIT_CONNECTION, REQUEST_CONTEXTS, UPDATE_CONTEXTS} from "./rootConstants";
import api from '../services/api'

export const state = {
    connection: null,
    namespaces: [],
    contexts: [],    
};

export const getters = {
    // tabs: state => {
    //     return state.tabs.map(tab  => {
    //         return {
    //             path: '/' + tab,
    //             title: tab.replace(/^\w/, c => c.toUpperCase()),
    //             isActive: rootState.route ? rootState.route.path.endsWith(tab) : true
    //         }
    //     })
    // },
};

export const mutations = {
    [INIT_CONNECTION] : (state, connection) => {
        state.connection = connection;
    },
};

export const actions = {
    [INIT_CONNECTION]: ({ commit }) => {
        let connection = new HubConnectionBuilder()
            .withUrl("http://localhost:5000/managerhub")
            .configureLogging(LogLevel.Debug)
            .build();
        connection.start().catch(err => console.error(err.toString()));
        commit(INIT_CONNECTION, connection);
    },
    
    [REQUEST_CONTEXTS]: ({ commit }) => {
        let newContexts = api.getContexts(); 
        commit(UPDATE_CONTEXTS, newContexts);
    },
};