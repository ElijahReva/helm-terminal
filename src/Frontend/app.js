import Vue from 'vue'

import App from './App.vue'
import Logger from 'js-logger'
import { sync } from 'vuex-router-sync'
import 'semantic-ui-css/semantic'
import 'semantic-ui-css/semantic.css'

import VueCodemirror from 'vue-codemirror'
import 'codemirror/lib/codemirror.css'

import Router from './router'
import Store from './store'




Logger.useDefaults();
Logger.setLevel(Logger.DEBUG);






Vue.use(VueCodemirror);

Vue.config.errorHandler = function(err) {
  Logger.error('Vue error: ', err)
};

let root = document.createElement('div');
root.setAttribute("id", "app");
document.body.appendChild(root);


let router = Router;
let store = Store;

const unsync = sync(store, router);

export default new Vue({
  el: '#app',
  template: '<App/>',
  store,
  router,
  components: { App }
})
