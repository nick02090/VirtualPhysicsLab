import userApi from "@/api/user.js";
import * as globalTypes from '../mutation-types'

const getters = {
    isLoggedIn(state) {
        return state.token == null ? false : true;
    },
    organizationUrl(state) {
        return state.organizationData.organizationUrl;
    },
    getUsers(state) {
        if (state.user) {
            return state.users.filter(x => x.id != state.user.id);
        }
        return state.users;
    }
}

// mutation-types
const types = {
    SET_LOADING: "SET_LOADING",
    SET_USER: "SET_USER",
    SET_TOKEN: "SET_TOKEN",
    SET_ORGANIZATION_DATA: "SET_ORGANIZATION_DATA",
    SET_PROFILE: "SET_PROFILE",
    SET_USERS: "SET_USERS"
}

// state
const state = {
    loading: false,
    user: null,
    organizationData: null,
    token: localStorage.getItem('token') || null,
    profile: null,
    users: []
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
    },
    [types.SET_ORGANIZATION_DATA](state, data) {
        state.organizationData = data;
    },
    [types.SET_PROFILE](state, data) {
        state.profile = data;
    },
    [types.SET_USERS](state, data) {
        state.users = data;
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
    async getUser({
        state,
        commit,
        dispatch,
        rootState
    }, id) {
        return new Promise(async (resolve, reject) => {
            try {
                commit(types.SET_LOADING, true)

                var userData = await userApi.getUser(id);
                commit(types.SET_PROFILE, userData);

                resolve();
            } catch (e) {
                reject(e);
            } finally {
                commit(types.SET_LOADING, false)
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
    },
    async retrieveOrganizationData({
        state,
        commit,
        dispatch,
        rootState
    }) {
        return new Promise(async (resolve, reject) => {
            try {

                let organizationData = await userApi.organizationData();
                commit(types.SET_ORGANIZATION_DATA, organizationData)


            } catch (e) {
                reject(e);
            }
        })
    },
    async getUsersAsync({
        state,
        commit,
        dispatch,
        rootState
    }) {
        return new Promise(async (resolve, reject) => {
            try {
                commit(types.SET_LOADING, true);

                let users = await userApi.getUsers();
                commit(types.SET_USERS, users);

                resolve();
            } catch (e) {
                reject(e)
            } finally {
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