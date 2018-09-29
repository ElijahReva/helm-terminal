import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter);

import Manager from './components/manager/Manager.vue'
import Watcher from './components/watcher/Watcher'
import Login from './components/Login.vue'

import Store from './store'

const ifNotAuthenticated = (to, from, next) => {
  if (!Store.getters.isAuthenticated) {
    next();
    return
  }
  next('/')
};

const ifAuthenticated = (to, from, next) => {
  if (Store.getters.isAuthenticated) {
    next();
    return
  }
  next('/login')
};

export default new VueRouter({
  mode: 'history',
  routes: [
    { 
      path: '/',
      redirect: { name: 'Manager' }
    },
    {
      path: '/manager',
      name: 'Manager',
      component: Manager,
      beforeEnter: ifAuthenticated
    },
    {
      path: '/watcher',
      name: 'Watcher',
      component: Watcher,
      beforeEnter: ifAuthenticated
    },
    {
      path: '/logs',
      name: 'Logs',
      component: Manager,
      beforeEnter: ifAuthenticated
    },
    {
      path: '/login',
      name: 'Login',
      component: Login,
      beforeEnter: ifNotAuthenticated
    },
    {
      path: '/logout',
      name: 'Logout',
      component: Login,
      beforeEnter: ifNotAuthenticated
    }
  ]
})
