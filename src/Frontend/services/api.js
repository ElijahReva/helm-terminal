import axios from 'axios'

let api = axios.create();

export default {    
    getContexts(api) {
        return api.get('contexts');            
    },
    
    getNamespaces(api, context) {
        return api.get('namespaces', {
            params: {
                context: context
            },
        });            
    },

    getCharts(api, context, namespace) {
        return api.get('charts', {
            params: {
                context: context,
                namespace: namespace,
            },
        });            
    },
}