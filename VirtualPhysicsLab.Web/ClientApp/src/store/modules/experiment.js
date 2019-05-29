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
            var meshLog = state.logs.find(x => x.name === name);
            if (meshLog === undefined) return [];
            return meshLog.log;
        }
    }
}

const types = {
    SET_SCENE: "SET_SCENE",
    SET_AXIS: "SET_AXIS",
    UPDATE_MESHES: "UPDATE_MESHES",
    DELETE_MESH: "DELETE_MESH",
    SET_HIGHLIGHT: "SET_HIGHLIGHT",
    SET_CAMERA: "SET_CAMERA",
    SET_GROUND: "SET_GROUND",
    SET_WALLS: "SET_WALLS",
    SET_CANVAS: "SET_CANVAS",
    SET_PLAYING: "SET_PLAYING",
    SET_MESH_IMPOSTORS: "SET_MESH_IMPOSTORS",
    UPDATE_MESH_IMPOSTORS: "UPDATE_MESH_IMPOSTORS",

    SET_PHYSICS_IMPOSTOR: "SET_PHYSICS_IMPOSTOR",
    SET_DRAG_BEHAVIOUR: "SET_DRAG_BEHAVIOUR",
    SET_LAST_POSITION: "SET_LAST_POSITION",
    SET_DELETED_MESH: "SET_DELETED_MESH",
    UPDATE_MESH_AXIS: "UPDATE_MESH_AXIS",
    DELETE_MESH_AXIS: "DELETE_MESH_AXIS",

    UPDATE_PHYSICS: "UPDATE_PHYSICS",
    UPDATE_MESH_PHYSICS: "UPDATE_MESH_PHYSICS",
    REMOVE_MESH_PHYSIC: "REMOVE_MESH_PHYSIC",
    DELETE_MESH_PHYSICS: "DELETE_MESH_PHYSICS",

    UPDATE_LOGS: "UPDATE_LOGS",
    SET_MESH_LOGS: "SET_MESH_LOGS",
    UPDATE_MESH_LOGS: "UPDATE_MESH_LOGS",
    DELETE_MESH_LOGS: "DELETE_MESH_LOGS"
}

const state = {
    scene: null,
    axis: [],
    meshes: [],
    highlight: null,
    camera: null,
    ground: null,
    walls: [],
    canvas: null,
    playing: true,
    physicsImpostors: [],

    dragBehaviour: [],
    physicsImpostor: null,
    lastPosition: null,
    deletedMesh: null,
    meshAxis: [],

    physics: [],

    logs: []
}

const mutations = {
    [types.SET_SCENE](state, data) {
        state.scene = data;
    },
    [types.SET_AXIS](state, data) {
        state.axis = data
    },
    [types.UPDATE_MESHES](state, data) {
        state.meshes.push(data)
    },
    [types.DELETE_MESH](state, data) {
        var allMeshes = state.meshes;
        var meshes = allMeshes.filter(x => x != data);
        state.meshes = meshes;
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
    [types.SET_WALLS](state, data) {
        state.walls = data
    },
    [types.SET_CANVAS](state, data) {
        state.canvas = data
    },
    [types.SET_PLAYING](state, data) {
        state.playing = data
    },
    [types.SET_MESH_IMPOSTORS](state, data) {
        state.physicsImpostors = data
    },
    [types.UPDATE_MESH_IMPOSTORS](state, data) {
        state.physicsImpostors.push(data);
    },

    [types.SET_PHYSICS_IMPOSTOR](state, data) {
        state.physicsImpostor = data;
    },
    [types.SET_DRAG_BEHAVIOUR](state, data) {
        state.dragBehaviour = data
    },
    [types.SET_LAST_POSITION](state, data) {
        state.lastPosition = data;
    },
    [types.SET_DELETED_MESH](state, data) {
        state.deletedMesh = data;
    },
    [types.UPDATE_MESH_AXIS](state, data) {
        state.meshAxis.push(data);
    },
    [types.DELETE_MESH_AXIS](state, data) {
        state.meshAxis = state.meshAxis.filter(x => x.mesh !== data);
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
                    if (duplicates[i].properties.axis === data.physic.properties.axis) {
                        duplicates[i].properties.value = data.physic.properties.value;
                        return;
                    }
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
    [types.DELETE_MESH_PHYSICS](state, data) {
        state.physics = state.physics.filter(x => x.name !== data);
    },

    [types.UPDATE_LOGS](state, data) {
        state.logs.push(data)
    },
    [types.SET_MESH_LOGS](state, data) {
        var meshLog = state.logs.find(x => x.name === data.mesh).log;
        meshLog.push(data.log);
    },
    [types.UPDATE_MESH_LOGS](state, data) {
        var arrayRemove = (arr, value) => {
            return arr.filter(function (ele) {
                return ele != value;
            });
        }
        var meshLog = state.logs.find(x => x.name === data.mesh).log;
        var group = meshLog.filter(x => x.type === data.log.type);
        var oldLog = group.find(x => x.key === data.log.key);
        if (oldLog.type === "physics") {
            switch (oldLog.key) {
                case "velocity":
                    oldLog = group.find(x => x.properties.axis === data.log.properties.axis && x.properties.time.start === data.log.properties.time.start);
                    break;
                default:
                    break;
            }
        }
        var newMeshLog = arrayRemove(meshLog, oldLog);
        newMeshLog.push(data.log);
        state.logs.find(x => x.name === data.mesh).log = newMeshLog;
    },
    [types.DELETE_MESH_LOGS](state, data) {
        state.logs = state.logs.filter(x => x.name !== data);
    }
}

const actions = {
    createWalls({
        state,
        commit,
        dispatch,
        rootState
    }) {
        let scene = state.scene;

        var northWall = new BABYLON.MeshBuilder.CreateGround("northWall", {
            width: 20,
            height: 20
        }, scene);
        northWall.position.z = 10;
        northWall.position.y = 9;
        northWall.rotation.x = -Math.PI / 2;
        northWall.physicsImpostor = new BABYLON.PhysicsImpostor(
            northWall,
            BABYLON.PhysicsImpostor.BoxImpostor, {
                mass: 0,
                friction: 0.1,
                restitution: 0.9
            },
            scene);
        northWall.visibility = false;

        var westWall = new BABYLON.MeshBuilder.CreateGround("westWall", {
            width: 20,
            height: 20
        }, scene);
        westWall.position.x = -10;
        westWall.position.y = 9;
        westWall.rotation.z = -Math.PI / 2;
        westWall.physicsImpostor = new BABYLON.PhysicsImpostor(
            westWall,
            BABYLON.PhysicsImpostor.BoxImpostor, {
                mass: 0,
                friction: 0.1,
                restitution: 0.9
            },
            scene);
        westWall.visibility = false;

        var eastWall = new BABYLON.MeshBuilder.CreateGround("eastWall", {
            width: 20,
            height: 20
        }, scene);
        eastWall.position.x = 10;
        eastWall.position.y = 9;
        eastWall.rotation.z = Math.PI / 2;
        eastWall.physicsImpostor = new BABYLON.PhysicsImpostor(
            eastWall,
            BABYLON.PhysicsImpostor.BoxImpostor, {
                mass: 0,
                friction: 0.1,
                restitution: 0.9
            },
            scene);
        eastWall.visibility = false;

        var southWall = new BABYLON.MeshBuilder.CreateGround("southWall", {
            width: 20,
            height: 20
        }, scene);
        southWall.position.z = -10;
        southWall.position.y = 9;
        southWall.rotation.x = Math.PI / 2;
        southWall.physicsImpostor = new BABYLON.PhysicsImpostor(
            southWall,
            BABYLON.PhysicsImpostor.BoxImpostor, {
                mass: 0,
                friction: 0.1,
                restitution: 0.9
            },
            scene);
        southWall.visibility = false;

        commit(types.SET_WALLS, [northWall, westWall, eastWall, southWall])
    },
    deleteWalls({
        state,
        commit,
        dispatch,
        rootState
    }) {
        let walls = state.walls;

        for (var i in walls) {
            walls[i].dispose();
        }

        commit(types.SET_WALLS, [])
    }
}

export default {
    namespaced: true,
    state,
    getters,
    actions,
    mutations
}