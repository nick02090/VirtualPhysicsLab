import * as globalTypes from '../mutation-types'

const getters = {
    getMeshByName(state) {
        return (name) => {
            return state.scene.getMeshByName(name);
        }
    },
    getMeshPhysic(state) {
        return (name) => {
            return state.physics.find(x => x.name === name).physic;
        }
    },
    getMeshLog(state) {
        return (name) => {
            return state.logs.find(x => x.name === name).log;
        }
    }
}

const types = {
    SET_SCENE: "SET_SCENE",
    UPDATE_MESHES: "UPDATE_MESHES",
    SET_HIGHLIGHT: "SET_HIGHLIGHT",
    SET_LIGHT: "SET_LIGHT",
    SET_CAMERA: "SET_CAMERA",
    SET_GROUND: "SET_GROUND",
    SET_CANVAS: "SET_CANVAS",
    SET_DRAG_BEHAVIOUR: "SET_DRAG_BEHAVIOUR",

    UPDATE_PHYSICS: "UPDATE_PHYSICS",
    UPDATE_MESH_PHYSICS: "UPDATE_MESH_PHYSICS",
    REMOVE_MESH_PHYSIC: "REMOVE_MESH_PHYSIC",

    UPDATE_LOGS: "UPDATE_LOGS",
    UPDATE_MESH_LOGS: "UPDATE_MESH_LOGS"
}

const state = {
    scene: null,
    meshes: [],
    highlight: null,
    light: null,
    camera: null,
    ground: null,
    canvas: null,
    dragBehaviour: [],

    physics: [],

    logs: []
}

const mutations = {
    [types.SET_SCENE](state, data) {
        state.scene = data;
    },
    [types.UPDATE_MESHES](state, data) {
        state.meshes.push(data)
    },
    [types.SET_HIGHLIGHT](state, data) {
        state.highlight = data
    },
    [types.SET_CAMERA](state, data) {
        state.camera = data
    },
    [types.SET_GROUND](state, data) {
        state.ground = data
    },
    [types.SET_LIGHT](state, data) {
        state.light = data
    },
    [types.SET_CANVAS](state, data) {
        state.canvas = data
    },
    [types.SET_DRAG_BEHAVIOUR](state, data) {
        state.dragBehaviour = data
    },

    [types.UPDATE_PHYSICS](state, data) {
        state.physics.push(data)
    },
    [types.UPDATE_MESH_PHYSICS](state, data) {
        var meshPhysic = state.physics.find(x => x.name === data.mesh).physic;
        var duplicates = meshPhysic.filter(x => x.name === data.physic.name);
        for (var i in duplicates) {
            switch (duplicates[i].name) {
                case "Brzina":
                    if (duplicates[i].properties.axis === data.physic.properties.axis) return;
                    break;
                default:
                    break;
            }
        }
        meshPhysic.push(data.physic);
    },
    [types.REMOVE_MESH_PHYSIC](state, data) {
        var meshPhysic = state.physics.find(x => x.name === data.mesh).physic;
        var duplicates = meshPhysic.filter(x => x.name === data.physic.name);
        var getRemoveable = () => {
            for (var i in duplicates) {
                switch (duplicates[i].name) {
                    case "Brzina":
                        if (duplicates[i].properties.axis === data.physic.properties.axis) {
                            return duplicates[i];
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        var arrayRemove = (arr, value) => {
            return arr.filter(function (ele) {
                return ele != value;
            });
        }
        var objToRemove = getRemoveable();
        if (objToRemove) {
            meshPhysic = arrayRemove(meshPhysic, objToRemove);
            state.physics.find(x => x.name === data.mesh).physic = meshPhysic;
        }
    },

    [types.UPDATE_LOGS](state, data) {
        state.logs.push(data)
    },
    [types.UPDATE_MESH_LOGS](state, data) {
        var meshLog = state.logs.find(x => x.name === data.mesh).log;
        meshLog.push(data.log);
    }
}

const actions = {

}

export default {
    namespaced: true,
    state,
    getters,
    actions,
    mutations
}