import * as meshTypes from "./mesh-types"
import box from "./box";
import sphere from "./sphere";
import store from "@/store/index.js"

export default {
    create(type, properties, material, scene) {
        var newMesh = null;
        switch (type) {
            case meshTypes.BOX:
                newMesh = box.create(properties, material, scene);
                break;
            case meshTypes.SPHERE:
                newMesh = sphere.create(properties, material, scene);
                break;
            default:
                console.error("Invalid type of mesh!");
                return;
        }
        this.addMeshLabel(newMesh);
        this.enableWallRestrictions(newMesh);
        var physic = {
            name: properties.name,
            physic: []
        }
        store.commit("experiment/UPDATE_PHYSICS", physic);
        var log = {
            name: properties.name,
            log: []
        }
        store.commit("experiment/UPDATE_LOGS", log);
        store.commit("experiment/UPDATE_MESHES", newMesh.name);
        return newMesh;
    },
    delete(mesh, name) {
        mesh.dispose();
        store.commit("experiment/DELETE_MESH", name);
        store.commit("experiment/DELETE_MESH_PHYSICS", name);
        store.commit("experiment/DELETE_MESH_AXIS", name);
        store.commit("experiment/DELETE_MESH_LOGS", name);
    },
    addMeshLabel(mesh) {
        var plane = this.makeTextPlane(mesh.name, "white", 1);
        plane.position.y = mesh.position.y + mesh.scaling.y + 1;
        plane.parent = mesh;
    },
    makeTextPlane(text, color, size) {
        var scene = store.state.experiment.scene;
        var dynamicTexture = new BABYLON.DynamicTexture("DynamicTexture", 50, scene, true);
        dynamicTexture.hasAlpha = true;
        dynamicTexture.drawText(text, 0, 40, "20px Arial", color, "transparent", true);
        var plane = new BABYLON.Mesh.CreatePlane("TextPlane", size, scene, true);
        plane.material = new BABYLON.StandardMaterial("TextPlaneMaterial", scene);
        plane.material.backFaceCulling = false;
        plane.material.specularColor = new BABYLON.Color3(0, 0, 0);
        plane.material.diffuseTexture = dynamicTexture;
        return plane;
    },
    toggleHightlight(mesh) {
        var hl = store.state.experiment.highlight;
        if (hl.hasMesh(mesh)) {
            hl.removeMesh(mesh);
        } else {
            hl.addMesh(mesh, BABYLON.Color3.Green());
        }
    },
    addDragBehaviour(mesh, axis, first) {
        if (first === true) {
            var physicsImpostor = {
                mass: mesh.physicsImpostor.mass,
                type: mesh.physicsImpostor.type
            };
            store.commit("experiment/SET_PHYSICS_IMPOSTOR", physicsImpostor);
            mesh.physicsImpostor.dispose();
            var lastPosition = {
                x: mesh.position.x,
                y: mesh.position.y,
                z: mesh.position.z
            };
            store.commit("experiment/SET_LAST_POSITION", lastPosition);
            store.dispatch("experiment/deleteWalls");
        }

        var dragBehaviours = store.state.experiment.dragBehaviour;
        this.removeDragBehaviours(mesh);
        var dragBehaviour = dragBehaviours[axis];
        mesh.addBehavior(dragBehaviour);
    },
    removeDragBehaviours(mesh, permanent) {
        var dragBehaviours = store.state.experiment.dragBehaviour;
        dragBehaviours.forEach(dragBehaviour => {
            mesh.removeBehavior(dragBehaviour);
        })

        if (permanent === true) {
            var scene = store.state.experiment.scene;
            var physicsImpostor = store.state.experiment.physicsImpostor;
            mesh.physicsImpostor = new BABYLON.PhysicsImpostor(
                mesh,
                physicsImpostor.type, {
                    mass: physicsImpostor.mass,
                },
                scene
            );
            store.commit("experiment/SET_PHYSICS_IMPOSTOR", null);
            store.dispatch("experiment/createWalls");
        }
    },
    enableWallRestrictions(mesh) {
        var scene = store.state.experiment.scene;
        var ground = store.state.experiment.ground;

        var groundSize = ground.getBoundingInfo().boundingBox.extendSize;
        var meshSize = mesh.getBoundingInfo().boundingBox.extendSize;

        var north = groundSize.x - meshSize.x;
        var south = -north;
        var east = groundSize.z - meshSize.z;
        var west = -east;

        scene.registerBeforeRender(function () {
            let physicsImpostor = store.state.experiment.physicsImpostor;
            if (!mesh.isDisposed()) {
                if (physicsImpostor !== null) {
                    if (mesh.position.x > north) {
                        mesh.position.x = north;
                    }
                    if (mesh.position.x < south) {
                        mesh.position.x = south;
                    }
                    if (mesh.position.z > east) {
                        mesh.position.z = east;
                    }
                    if (mesh.position.z < west) {
                        mesh.position.z = west;
                    }
                    if (mesh.position.y < 0) {
                        mesh.position.y = 0;
                    }
                }
                if (mesh.position.y < -30) {
                    mesh.dispose();
                    store.commit("experiment/DELETE_MESH", mesh.name);
                    store.commit("experiment/SET_DELETED_MESH", mesh.name);
                }
            }
        })
    },
    checkCollisions(mesh, name) {
        var allMeshes = store.state.experiment.meshes;
        var meshes = allMeshes.filter(x => x != name);
        for (var i in meshes) {
            var obj = store.getters["experiment/getMeshByName"](meshes[i]);
            if (mesh.intersectsMesh(obj, false)) {
                return true;
            }
        }
        return false;
    },
    setMeshToLastPosition(mesh) {
        var lastPosition = store.state.experiment.lastPosition;
        mesh.position = lastPosition;
    }
}