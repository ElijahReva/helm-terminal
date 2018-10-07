import {HubConnectionBuilder, LogLevel} from '@aspnet/signalr'
import {
    INIT_API,
    INIT_SIGNALR,
    INIT_CONNECTION,
    REQUEST_CONTEXTS,
    UPDATE_CONTEXTS,
    REQUEST_IN_PROGRESS,
    SET_CONTEXT, SET_NAMESPACE, UPDATE_NAMESPACES, REQUEST_NAMESPACES
} from "./rootConstants";
import api from '../services/api'
import axios from "axios";

export const state = {
    isRequestInProgress: false,
    
    signalR: null,
    api: null,
    
    currentContext: null,
    contexts: [],
    
    namespaces: [],
    currentNamespace: null,
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
    
    [INIT_API] : (state, api) => {
        state.api = api;
    },  
    
    [INIT_SIGNALR] : (state, signalR) => {
        state.signalR = signalR;
    },
    
    [UPDATE_CONTEXTS] : (state, newContexts) => {
        state.contexts = newContexts;
    },    
    
    [UPDATE_NAMESPACES] : (state, newNamespaces) => {
        state.namespaces = newNamespaces;
    },
    
    [REQUEST_IN_PROGRESS] : (state, isRequestInProgress) => {
        state.isRequestInProgress = isRequestInProgress;
    },
    
    [SET_CONTEXT] : (state, newContext) => {
        state.currentContext = newContext;
    },
    
    [SET_NAMESPACE] : (state, newNs) => {
        state.currentNamespace = newNs;
    },
};

export const actions = {
    [INIT_CONNECTION]: ({ commit, state, dispatch }) => {
        
        let api = axios.create({
            baseURL: `/api/`,
            withCredentials: false,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }

        });
        
        api.interceptors.request.use(function (config) {
            commit(REQUEST_IN_PROGRESS, true);
            return config;
        }, function (error) {
            commit(REQUEST_IN_PROGRESS, false);
            return Promise.reject(error);
        });
        
        api.interceptors.response.use(function (response) {
            commit(REQUEST_IN_PROGRESS, false);
            return response;
        }, function (error) {
            commit(REQUEST_IN_PROGRESS, false);
            return Promise.reject(error);
        });

        commit(INIT_API, api);
        
        
        
        let signalR = new HubConnectionBuilder()
            .withUrl("/managerhub")
            .configureLogging(LogLevel.Debug)
            .build();
        
        signalR
            .start()
            .catch(err => console.error(err.toString()));
        
        commit(INIT_SIGNALR, signalR);        
    },
    
    [SET_CONTEXT] : ({ commit }, newContext) => {
        commit(SET_CONTEXT, newContext)    
    },

    [SET_NAMESPACE] : ({ commit }, newNs) => {
        commit(SET_NAMESPACE, newNs)    
    },

    [REQUEST_CONTEXTS]: ({ state, commit }) => {
        // commit(REQUEST_IN_PROGRESS, true);
        api.getContexts(state.api)
            .then(resp => {
                commit(UPDATE_CONTEXTS, resp.data);
                // if resp data has no old context select first
                // if currentCtx = null select first
                commit(SET_CONTEXT, resp.data[0].name);
            })
            .catch(err => console.log(err));
        // commit(REQUEST_IN_PROGRESS, false);       
    },
    
    [REQUEST_NAMESPACES] : ({ state, commit, dispatch}) => {
        
        api.getNamespaces(state.api, state.currentContext)
            .then(resp => {
                commit(UPDATE_NAMESPACES, resp.data);
                dispatch(SET_NAMESPACE, resp.data[0].name);
            })
            .catch(err => console.log(err));
    },
    
};