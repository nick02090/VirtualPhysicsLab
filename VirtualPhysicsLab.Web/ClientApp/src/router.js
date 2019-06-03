import Vue from 'vue'
import Router from 'vue-router'
import Home from './views/Home.vue'
import Experiment from './views/Experiment.vue'
import Profile from './views/Profile.vue'
import Login from './views/Login.vue'
import Register from './views/Register.vue'

Vue.use(Router);

export default new Router({
    mode: 'history',
    routes: [{
            path: '',
            component: Home,
            name: 'home'
        },
        {
            path: '/home',
            redirect: '/'
        },
        {
            path: '/experiment',
            component: Experiment,
            name: 'experiment'
        },
        {
            path: '/profile',
            component: Profile,
            name: 'profile'
        },
        {
            path: '/login',
            component: Login,
            name: 'login'
        },
        {
            path: '/register',
            component: Register,
            name: 'register'
        },
        {
            path: '*',
            redirect: '/'
        }
    ]
})