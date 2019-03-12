import Vue from 'vue'
import Router from 'vue-router'
import App from './App.vue'
import Home from './views/Home.vue'

Vue.use(Router);

export default new Router({
    mode: 'history',
    routes: [{
        path: '',
        component: App,
        children: [{
                path: '',
                component: Home,
                name: 'home'
            },
            {
                path: '/home',
                redirect: '/'
            }
        ]
    }]
})