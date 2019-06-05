import userApi from "@/api/user.js";
import * as globalTypes from '../mutation-types'

const getters = {}
// mutation-types
const types = {}

// state
const state = {}

const mutations = {}

// actions
const actions = {
    async postUser({
        state,
        commit,
        dispatch,
        rootState
    }, id) {
        return new Promise(async (resolve, reject) => {
            try {

                var userData = await userApi.postUser();

                resolve();
            } catch (e) {
                reject(e);
            }
        })
    }
}


export default {
    namespaced: true,
    state,
    getters,
    actions,
    mutations
}