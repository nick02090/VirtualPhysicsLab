import Vue from 'vue'
import App from './App.vue'
import Buefy from 'buefy'
import 'buefy/dist/buefy.css'
import Babylon from 'babylonjs'
import router from './router'
import store from './store/index.js'
import VeeValidate from 'vee-validate'

Vue.config.productionTip = false

Vue.use(Buefy, {
  defaultIconPack: 'fa'
});

Vue.use(Babylon);

Vue.use(VeeValidate);

new Vue({
  router,
  store,
  render: h => h(App),
}).$mount('#app')