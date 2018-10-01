import axios from 'axios'

export default {
    api() {
        return axios.create({
            baseURL: `/api/`,
            withCredentials: false,
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }})
    },
    
    getContexts() {
        return api().get('contexts')    
    }
}