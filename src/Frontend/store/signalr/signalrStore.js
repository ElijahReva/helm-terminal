import { } from './constants'
import {INIT_CONNECTION} from "./constants";
import {HubConnectionBuilder, LogLevel} from '@aspnet/signalr'


export const state = {    
    connection: null
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
            .withUrl("http://localhost:8080/managerhub")
            .configureLogging(LogLevel.Debug)
            .build();
        connection.start().catch(err => console.error(err.toString()));
        console.log("tetse2")
        commit(INIT_CONNECTION, connection);
    }
};
