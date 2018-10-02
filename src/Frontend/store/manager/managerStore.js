import {
    UPDATE_YAML,
    UPDATE_CHART,
    CHANGE_YAML_VISIBILITY,
    CHART_SELECTED,
    UPDATE_ERRORS,
    VALIDATE_YAML
} from "./actions";
import {safeLoad} from "js-yaml";


export const state = {
    charts: [
        "env-man",
        "watcher",
        "logs",
    ],
    currentYaml: "yaml:\n  test: qw123\n",
    currentChart: "env-man",
    currentActions: [
        { text: 'dd', type: "submit", color: "blue"},
        { text: 'dd2', type: "submit", color: "red"},
        { text: 'dd4', type: "submit", color: "green"},
    ],
    isYamlVisible: true,
    whyYouCant: ""
};

export const getters = {
    
    yamlButtonColor: state => state.isYamlVisible ? "gray" : "green",
    
    yamlButtonText: state => state.isYamlVisible ? "Hide yaml" : "Show yaml",
    
    hasErrors: state => state.whyYouCant !== "",
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
    
    [UPDATE_CHART] : (state, newChart) => {
        state.currentChart = newChart;
    },
};

export const actions = {
    
    [CHART_SELECTED] : ({ commit, state }, newChart) => {        
        commit(UPDATE_YAML, "yaml:\n  new: test\n");
        commit(UPDATE_CHART, newChart);
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
