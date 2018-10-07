import axios from 'axios'

export default {    
    getContexts(api) {
        return api.get('contexts');            
    }
}