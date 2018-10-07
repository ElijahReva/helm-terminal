import {
    UPDATE_YAML,
    SET_CHARTS,
    CHANGE_YAML_VISIBILITY,
    SELECT_CHART,
    UPDATE_ERRORS,
    VALIDATE_YAML, 
    UPDATE_CHARTS,
} from "./actions";
import {safeLoad} from "js-yaml";
import api from '../../services/api'

export const state = {
    charts: null,
    currentYaml: null,
    currentChart: null,
    currentActions: [
        { text: 'Install', type: "submit", color: "blue"},
        { text: 'Lint', type: "submit", color: "green"},
        { text: 'Dry-run', type: "submit", color: "green"},
    ],
    isYamlVisible: true,
    whyYouCant: ""
};

export const getters = {
    
    yamlButtonColor: state => state.isYamlVisible ? "gray" : "green",
    
    yamlButtonText: state => state.isYamlVisible ? "Hide yaml" : "Show yaml",
    
    hasErrors: state => state.whyYouCant !== "",

    getCharts: state => state.charts.map(chart => {
        chart['isSelected'] = state.currentChart === chart.name;
        return chart
    }),
};

export const mutations = {

    [CHANGE_YAML_VISIBILITY] : state => {
        state.isYamlVisible = !state.isYamlVisible;
    },
    
    [UPDATE_YAML] : (state, yaml) => {
        state.currentYaml = yaml;
    },    
    
    [UPDATE_ERRORS] : (state, error) => {
        state.whyYouCant = error;
    },    
    
    [SELECT_CHART] : (state, newChart) => {
        state.currentChart = newChart;
    },
    
    [SET_CHARTS] : (state, charts) => {
        state.charts = charts;
    },
};

export const actions = {
    
    [UPDATE_CHARTS] : ({ dispatch, commit, state, rootState }) => {
        api.getCharts(rootState.api, rootState.currentContext, rootState.currentNamespace)
            .then(resp => {
                commit(SET_CHARTS, resp.data);
                dispatch(SELECT_CHART, resp.data[0].name);
            })
            .catch(err => console.log(err))
        
    },  
    
    [SELECT_CHART] : ({ commit, state }, newChart) => {        
        commit(UPDATE_YAML, "yaml:\n  new: test\n");
        commit(SELECT_CHART, newChart);
    },
    
    [VALIDATE_YAML] : ({ commit, state }, newYaml) => {
        try {
            
            const doc = safeLoad(newYaml);                     
            commit(UPDATE_ERRORS, "");
        } catch (e) {
            commit(UPDATE_ERRORS, e.message)
        }
        commit(UPDATE_YAML, newYaml);
    },
};
