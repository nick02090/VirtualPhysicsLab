import userApi from "@/api/user.js";
import * as globalTypes from '../mutation-types'

const getters = {
    isLoggedIn(state) {
        return state.token == null ? false : true;
    }
}

// mutation-types
const types = {
    SET_LOADING: "SET_LOADING",
    SET_USER: "SET_USER",
    SET_TOKEN: "SET_TOKEN"
}

// state
const state = {
    loading: false,
    user: null,
    token: localStorage.getItem('token') || null
}

const mutations = {
    [types.SET_LOADING](state, data) {
        state.loading = data;
    },
    [types.SET_USER](state, data) {
        state.user = data;
    },
    [types.SET_TOKEN](state, data) {
        state.token = data;
    }
}

// actions
const actions = {
    async createUser({
        state,
        commit,
        dispatch,
        rootState
    }, user) {
        return new Promise(async (resolve, reject) => {
            try {
                commit(types.SET_LOADING, true);

                var userData = await userApi.createUser(user);
                commit(types.SET_USER, userData);

                resolve();
            } catch (e) {
                reject(e);
            } finally {
                commit(types.SET_LOADING, false);
            }
        })
    },
    async authenticate({
        state,
        commit,
        dispatch,
        rootState
    }, user) {
        return new Promise(async (resolve, reject) => {
            try {
                commit(types.SET_LOADING, true);

                var userData = await userApi.authenticate(user);
                if (!userData) {
                    resolve(false);
                }
                commit(types.SET_USER, userData);

                const token = userData.token;
                localStorage.setItem('token', token);
                commit(types.SET_TOKEN, token);

                resolve(true);
            } catch (e) {
                reject(e);
            } finally {
                commit(types.SET_LOADING, false);
            }
        })
    },
    async checkAvailability({
        state,
        commit,
        dispatch,
        rootState
    }, user) {
        return new Promise(async (resolve, reject) => {
            try {
                commit(types.SET_LOADING, true);

                var isAvailable = await userApi.checkAvailability(user);

                resolve(isAvailable);
            } catch (e) {
                reject(e);
            } finally {
                commit(types.SET_LOADING, false);
            }
        })
    },
    async retrieveUser({
        state,
        commit,
        dispatch,
        rootState
    }) {
        return new Promise(async (resolve, reject) => {
            try {
                commit(types.SET_LOADING, true);

                let token = state.token;
                if (token != null) {
                    var userData = await userApi.retrieveUser(token);

                    commit(types.SET_USER, userData);
                }

                resolve();
            } catch (e) {
                reject(e);
            } finally {
                commit(types.SET_LOADING, false);
            }
        })
    },
    async logout({
        state,
        commit,
        dispatch,
        rootState
    }) {
        return new Promise(async (resolve, reject) => {
            try {
                commit(types.SET_LOADING, true);

                let token = state.token;
                await userApi.logout(token);

                resolve()
            } catch (e) {
                reject(e);
            } finally {
                localStorage.removeItem('token');
                commit(types.SET_TOKEN, null);
                commit(types.SET_LOADING, false);
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