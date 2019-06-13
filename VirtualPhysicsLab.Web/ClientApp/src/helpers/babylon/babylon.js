import mesh from "./mesh/mesh"
import store from "@/store/index.js"
import physics from "./physics/physics"

export default {
    toggleHighlight(obj) {
        mesh.toggleHightlight(obj);
    },
    addDragBehaviour(obj, axis, first) {
        mesh.addDragBehaviour(obj, axis, first);
    },
    removeDragBehaviours(obj, permanent) {
        mesh.removeDragBehaviours(obj, permanent);
    },
    addMesh(type, properties, material, scene) {
        return mesh.create(type, properties, material, scene);
    },
    findEmptyPosition(obj) {
        mesh.findEmptyPosition(obj);
    },
    addPhysic(obj, physic) {
        physics.addPhysic(obj, physic);
    },
    checkCollisions(obj, name) {
        return mesh.checkCollisions(obj, name);
    },
    getCollisions(obj, name) {
        return mesh.getCollisions(obj, name);
    },
    setMeshToLastPosition(obj) {
        mesh.setMeshToLastPosition(obj);
    },
    deleteMesh(obj, name) {
        mesh.delete(obj, name);
    },
    togglePlay(data) {
        if (data === false) {
            var existingMeshes = store.state.experiment.meshes;
            var physicsImpostors = [];
            for (var i in existingMeshes) {
                var mesh = store.getters["experiment/getMeshByName"](existingMeshes[i]);
                var physicsImpostor = {
                    name: existingMeshes[i],
                    mass: mesh.physicsImpostor.mass,
                    type: mesh.physicsImpostor.type,
                    velocity: mesh.physicsImpostor.getLinearVelocity(),
                    friction: mesh.physicsImpostor.friction,
                    restitution: mesh.physicsImpostor.restitution
                };
                physicsImpostors.push(physicsImpostor);
                mesh.physicsImpostor.dispose();
            }
            store.commit("experiment/SET_MESH_IMPOSTORS", physicsImpostors);
        } else {
            var scene = store.state.experiment.scene;
            var ground = store.state.experiment.ground;
            var groundImpostor = {
                mass: ground.physicsImpostor.mass,
                type: ground.physicsImpostor.type,
                friction: ground.physicsImpostor.friction,
                restitution: ground.physicsImpostor.restitution
            };
            store.dispatch("experiment/deleteWalls");
            scene.disablePhysicsEngine();
            var gravityVector = store.state.experiment.gravity;
            var physicsPlugin = new BABYLON.CannonJSPlugin();
            scene.enablePhysics(gravityVector, physicsPlugin);
            ground.physicsImpostor = new BABYLON.PhysicsImpostor(
                ground,
                groundImpostor.type, {
                    mass: groundImpostor.mass,
                    friction: groundImpostor.friction,
                    restitution: groundImpostor.restitution
                },
                scene
            );
            store.commit("experiment/SET_GROUND", ground);
            store.dispatch("experiment/createWalls");
            var physicsImpostors = store.state.experiment.physicsImpostors;
            for (var i in physicsImpostors) {
                var obj = store.getters["experiment/getMeshByName"](physicsImpostors[i].name);
                if (obj) {
                    obj.physicsImpostor = new BABYLON.PhysicsImpostor(
                        obj,
                        physicsImpostors[i].type, {
                            mass: physicsImpostors[i].mass,
                            friction: physicsImpostors[i].friction,
                            restitution: physicsImpostors[i].restitution
                        },
                        scene
                    );
                    obj.physicsImpostor.setLinearVelocity(new BABYLON.Vector3(parseFloat(physicsImpostors[i].velocity.x), parseFloat(physicsImpostors[i].velocity.y), parseFloat(physicsImpostors[i].velocity.z)));
                }
            }
        }
    },
    showMeshAxis(name) {
        var obj = store.getters["experiment/getMeshByName"](name);
        var scene = store.state.experiment.scene;
        var start = new BABYLON.Vector3.Zero(); // because mesh is their parent, they become locally assigned to it
        var axis = this.createAxis(start, 2, scene);
        for (var i in axis) {
            axis[i].axis.parent = obj;
            axis[i].char.parent = obj;
        }
        var meshAxis = {
            mesh: name,
            axis: axis
        };
        store.commit("experiment/UPDATE_MESH_AXIS", meshAxis);
    },
    deleteMeshAxis(name) {
        var meshAxis = store.state.experiment.meshAxis.find(x => x.mesh === name);
        for (var i in meshAxis.axis) {
            var line = meshAxis.axis[i].axis;
            var char = meshAxis.axis[i].char;
            line.dispose();
            char.dispose();
        }
        store.commit("experiment/DELETE_MESH_AXIS", name);
    },
    showAxis() {
        var scene = store.state.experiment.scene;
        var ground = store.state.experiment.ground;
        var groundSize = ground.getBoundingInfo().boundingBox.extendSize;
        var x = groundSize.x;
        var z = groundSize.z;
        var start = new BABYLON.Vector3(-x, -1, -z);
        var axis = this.createAxis(start, 5, scene);
        store.commit("experiment/SET_AXIS", axis);
    },
    removeAxis() {
        var axis = store.state.experiment.axis;
        for (var i in axis) {
            var line = axis[i].axis;
            var char = axis[i].char;
            line.dispose();
            char.dispose();
        }
        store.commit("experiment/SET_AXIS", []);
    },
    createAxis(start, size, scene) {
        var makeTextPlane = function (text, color, size, scene) {
            var dynamicTexture = new BABYLON.DynamicTexture("DynamicTexture", 50, scene, true);
            dynamicTexture.hasAlpha = true;
            dynamicTexture.drawText(text, 5, 40, "bold 36px Arial", color, "transparent", true);
            var plane = new BABYLON.Mesh.CreatePlane("TextPlane", size, scene, true);
            plane.material = new BABYLON.StandardMaterial("TextPlaneMaterial", scene);
            plane.material.backFaceCulling = false;
            plane.material.specularColor = new BABYLON.Color3(0, 0, 0);
            plane.material.diffuseTexture = dynamicTexture;
            return plane;
        };

        var axisX = BABYLON.Mesh.CreateLines("axisX", [
            start, new BABYLON.Vector3(start.x + size, start.y + 0, start.z + 0), new BABYLON.Vector3(start.x + size * 0.95, start.y + 0.05 * size, start.z + 0),
            new BABYLON.Vector3(start.x + size, start.y + 0, start.z + 0), new BABYLON.Vector3(start.x + size * 0.95, start.y + -0.05 * size, start.z + 0)
        ], scene);
        axisX.color = new BABYLON.Color3(1, 0, 0);
        var xChar = makeTextPlane("X", "red", size / 10, scene);
        xChar.position = new BABYLON.Vector3(start.x + 0.9 * size, start.y + 0.05 * size, start.z + 0);

        var axisY = BABYLON.Mesh.CreateLines("axisY", [
            start, new BABYLON.Vector3(start.x + 0, start.y + size, start.z + 0), new BABYLON.Vector3(start.x + -0.05 * size, start.y + size * 0.95, start.z + 0),
            new BABYLON.Vector3(start.x + 0, start.y + size, start.z + 0), new BABYLON.Vector3(start.x + 0.05 * size, start.y + size * 0.95, start.z + 0)
        ], scene);
        axisY.color = new BABYLON.Color3(0, 1, 0);
        var yChar = makeTextPlane("Y", "green", size / 10, scene);
        yChar.position = new BABYLON.Vector3(start.x + 0, start.y + 0.9 * size, start.z + -0.05 * size);

        var axisZ = BABYLON.Mesh.CreateLines("axisZ", [
            start, new BABYLON.Vector3(start.x + 0, start.y + 0, start.z + size), new BABYLON.Vector3(start.x + 0, start.y + -0.05 * size, start.z + size * 0.95),
            new BABYLON.Vector3(start.x + 0, start.y + 0, start.z + size), new BABYLON.Vector3(start.x + 0, start.y + 0.05 * size, start.z + size * 0.95)
        ], scene);
        axisZ.color = new BABYLON.Color3(0, 0, 1);
        var zChar = makeTextPlane("Z", "blue", size / 10, scene);
        zChar.position = new BABYLON.Vector3(start.x + 0, start.y + 0.05 * size, start.z + 0.9 * size);

        var x = {
            axis: axisX,
            char: xChar
        };
        var y = {
            axis: axisY,
            char: yChar
        };
        var z = {
            axis: axisZ,
            char: zChar
        };
        var axis = [x, y, z];
        return axis;
    },
    loadScene(scene, meshes, experiment) {
        for (var i in meshes) {
            var mesh = meshes[i];
            var type = "";
            switch (mesh.type) {
                case 2:
                    type = "BOX";
                    break;
                case 1:
                    type = "SPHERE";
                    break;
                case 7:
                    type = "CYLINDER";
                    break;
                default:
                    break;
            }
            var properties = {
                name: mesh.name,
                mass: mesh.settings.mass,
                friction: mesh.settings.friction,
                restitution: mesh.settings.restitution
            }
            var material = new BABYLON.StandardMaterial("material", scene);
            material.diffuseColor = new BABYLON.Color3(
                mesh.settings.color.r,
                mesh.settings.color.g,
                mesh.settings.color.b
            );
            var obj = this.addMesh(type, properties, material, scene);
            obj.position.x = mesh.settings.position.x;
            obj.position.y = mesh.settings.position.y;
            obj.position.z = mesh.settings.position.z;
            if (mesh.settings.axis == true) {
                this.showMeshAxis(mesh.name)
            }

            var pi = Math.PI;
            var x = mesh.settings.rotation.x * (pi / 180);
            var y = mesh.settings.rotation.y * (pi / 180);
            var z = mesh.settings.rotation.z * (pi / 180);
            var rotation = {
                x: x,
                y: y,
                z: z
            };
            var quaternion = new BABYLON.Quaternion.RotationYawPitchRoll(
                rotation.y,
                rotation.x,
                rotation.z
            );
            obj.rotationQuaternion = quaternion;
            obj.scaling = new BABYLON.Vector3(mesh.settings.size.x, mesh.settings.size.y, mesh.settings.size.z);
            obj.physicsImpostor.setLinearVelocity(new BABYLON.Vector3(parseFloat(mesh.settings.velocity.x), parseFloat(mesh.settings.velocity.y), parseFloat(mesh.settings.velocity.z)));

            var meshLog = {
                mesh: mesh.name,
                log: {
                    name: "Stvoren element",
                    icon: "fas fa-plus",
                    type: "life",
                    key: mesh.name,
                    description: `(${mesh.name})`,
                    properties: {
                        time: Date.now(),
                        position: obj.position
                    }
                }
            };
            store.commit("experiment/SET_MESH_LOGS", meshLog);
        }

        store.dispatch("experiment/deleteWalls");
        store.state.experiment.ground.scaling = new BABYLON.Vector3(parseFloat(experiment.settings.size.x), parseFloat(experiment.settings.size.y), parseFloat(experiment.settings.size.z))
        store.state.experiment.ground.physicsImpostor.friction = experiment.settings.friction;
        store.state.experiment.ground.physicsImpostor.restitution = experiment.settings.restitution;
        store.dispatch("experiment/createWalls");

        var gravity = experiment.settings.gravity;
        store.commit("experiment/SET_GRAVITY_AXIS", gravity.axis);
        var gravityVector = new BABYLON.Vector3(gravity.x, gravity.y, gravity.z)
        store.commit("experiment/SET_GRAVITY", gravityVector);

        if (experiment.settings.axis === false) {
            this.removeAxis();
        }
        if (experiment.settings.walls === false) {
            store.dispatch("experiment/deleteWalls");
        }
    },
    createEmptyScene(canvas, engine, polygon) {
        var scene = new BABYLON.Scene(engine);

        var camera = new BABYLON.ArcRotateCamera(
            "camera",
            0,
            0,
            30,
            BABYLON.Vector3.Zero(),
            scene
        );
        camera.attachControl(canvas, true);
        camera.setPosition(new BABYLON.Vector3(10, 6, -20));
        camera.lowerRadiusLimit = 15;
        camera.upperRadiusLimit = 35;
        camera.wheelDeltaPercentage = 0.01;

        store.commit("experiment/SET_CAMERA", camera);

        var hemLight1 = new BABYLON.HemisphericLight(
            "hemLight1",
            new BABYLON.Vector3(-0.5, 1, 0.5),
            scene
        );
        hemLight1.intensity = 0.3;

        var hemLight2 = new BABYLON.HemisphericLight(
            "hemLight2",
            new BABYLON.Vector3(0.5, 1, -0.5),
            scene
        );
        hemLight2.intensity = 0.3;

        var gravityVector = new BABYLON.Vector3(0, -9.81, 0);
        var physicsPlugin = new BABYLON.CannonJSPlugin();
        scene.enablePhysics(gravityVector, physicsPlugin);

        store.commit("experiment/SET_GRAVITY", gravityVector);
        store.commit("experiment/SET_GRAVITY_AXIS", 1);

        var ground = new BABYLON.MeshBuilder.CreateBox("ground", {
            width: polygon,
            height: 1,
            depth: polygon
        }, scene);
        ground.position.y = -1.5;
        ground.physicsImpostor = new BABYLON.PhysicsImpostor(
            ground,
            BABYLON.PhysicsImpostor.BoxImpostor, {
                mass: 0,
                friction: 0.1,
                restitution: 0.9
            },
            scene);

        store.commit("experiment/SET_GROUND", ground);

        var highlight = new BABYLON.HighlightLayer("highlight", scene);
        store.commit("experiment/SET_HIGHLIGHT", highlight);

        store.commit("experiment/SET_CANVAS", canvas);

        var xDragBehaviour = new BABYLON.PointerDragBehavior({
            dragAxis: new BABYLON.Vector3(1, 0, 0)
        });
        xDragBehaviour.useObjectOrienationForDragging = false;
        var yDragBehaviour = new BABYLON.PointerDragBehavior({
            dragAxis: new BABYLON.Vector3(0, 1, 0)
        });
        yDragBehaviour.useObjectOrienationForDragging = false;
        var zDragBehaviour = new BABYLON.PointerDragBehavior({
            dragAxis: new BABYLON.Vector3(0, 0, 1)
        });
        zDragBehaviour.useObjectOrienationForDragging = false;
        store.commit("experiment/SET_DRAG_BEHAVIOUR", [xDragBehaviour, yDragBehaviour, zDragBehaviour]);

        var groundSize = ground.getBoundingInfo().boundingBox.extendSize;
        var x = groundSize.x;
        var z = groundSize.z;
        var start = new BABYLON.Vector3(-x, -1, -z);
        var axis = this.createAxis(start, 5, scene);
        store.commit("experiment/SET_AXIS", axis);

        store.commit("experiment/SET_PLAYING", true);

        return scene;
    }
}