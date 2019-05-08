import Vue from 'vue'
import Vuex from 'vuex'
import getters from './getters'
import * as actions from './actions'
import mutations from './mutations'
import experiment from './modules/experiment'

Vue.use(Vuex);

const state = {

}

export default new Vuex.Store({
    // strict: true,
    getters,
    actions,
    mutations,
    modules: {
        experiment
    }
})