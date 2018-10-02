import { AUTH_REQUEST, AUTH_SUCCESS, AUTH_ERROR, AUTH_LOGOUT } from './actions'

// initial state
export const state = {
  token: localStorage.getItem('user-token') || '',
  status: '',
  hasLoadedOnce: false
};

// getters
export const getters = {
  isAuthenticated: state => !!state.token,
  authStatus: state => state.status
};

// mutations
export const mutations = {
  [AUTH_REQUEST]: state => {
    state.status = 'loading'
  },
  [AUTH_SUCCESS]: (state, resp) => {
    state.status = 'success';
    state.token = resp.token;
    state.hasLoadedOnce = true
  },
    
  [AUTH_ERROR]: state => {
    state.status = 'error';
    state.hasLoadedOnce = true
  },
    
  [AUTH_LOGOUT]: state => {
    state.token = ''
  }
};

// actions
export const actions = {
  [AUTH_REQUEST]: ({ commit }) => {
    commit(AUTH_REQUEST);
    apiCall({ url: 'auth', data: user, method: 'POST' })
       .then(resp => {
                let token = 'TestToken';           
                localStorage.setItem('user-token', token);
                axios.defaults.headers.common['Authorization'] = resp.token
                commit(AUTH_SUCCESS, token)
                dispatch(USER_REQUEST)
                resolve(resp)
      }).catch(err => {
        commit(AUTH_ERROR, err)
        localStorage.removeItem('user-token')
        reject(err)
      });
  },
  [AUTH_LOGOUT]: ({ commit }) => {
    commit(AUTH_LOGOUT);
    localStorage.removeItem('user-token')
  }
};
