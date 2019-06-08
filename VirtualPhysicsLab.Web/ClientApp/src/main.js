import Vue from 'vue'
import App from './App.vue'
import Buefy from 'buefy'
import 'buefy/dist/buefy.css'
import Babylon from 'babylonjs'
import router from './router'
import store from './store/index.js'
import VeeValidate from 'vee-validate'
import VueApexCharts from 'vue-apexcharts';
import excel from 'vue-excel-export';

Vue.config.productionTip = false

Vue.use(Buefy, {
  defaultIconPack: 'fa'
});

// Vue.use(Babylon);

Vue.use(VeeValidate);

Vue.use(excel);

Vue.component('apexchart', VueApexCharts);

router.beforeEach(async (to, from, next) => {
  if (store.state.user.user == null) {
    await store.dispatch("user/retrieveUser")
  }
  if (to.meta.guestRequired === true) {
    if (store.getters["user/isLoggedIn"]) {
      next('/profile');
      return;
    }
  }
  if (to.meta.userRequired === true) {
    var userId = to.query["id"]
    if (userId) {
      next();
      return;
    }
    if (!store.getters["user/isLoggedIn"]) {
      next('/login');
      return;
    }
  }
  next();
})

new Vue({
  router,
  store,
  render: h => h(App),
}).$mount('#app')