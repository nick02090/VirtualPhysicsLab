import Vue from 'vue'
import Router from 'vue-router'
import Home from './views/Home.vue'
import Experiment from './views/Experiment.vue'
import Profile from './views/Profile.vue'
import Info from './views/Info.vue'

Vue.use(Router);

export default new Router({
    mode: 'history',
    routes: [{
        path: '',
        component: Home,
        name: 'home'
    }, {
        path: '/home',
        redirect: '/'
    }, {
        path: '/experiment',
        component: Experiment,
        name: 'experiment'
    }, {
        path: '/profile',
        component: Profile,
        name: 'profile'
    }, {
        path: '/info',
        component: Info,
        name: 'info'
    }, {
        path: '*',
        redirect: '/'
    }]
})