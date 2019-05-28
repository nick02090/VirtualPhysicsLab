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
                    velocity: mesh.physicsImpostor.getLinearVelocity()
                };
                physicsImpostors.push(physicsImpostor);
                mesh.physicsImpostor.dispose();
            }
            store.commit("experiment/SET_MESH_IMPOSTORS", physicsImpostors);
        } else {
            var scene = store.state.experiment.scene;
            var physicsImpostors = store.state.experiment.physicsImpostors;
            for (var i in physicsImpostors) {
                var obj = store.getters["experiment/getMeshByName"](physicsImpostors[i].name);
                obj.physicsImpostor = new BABYLON.PhysicsImpostor(
                    obj,
                    physicsImpostors[i].type, {
                        mass: physicsImpostors[i].mass,
                    },
                    scene
                );
                obj.physicsImpostor.setLinearVelocity(new BABYLON.Vector3(parseFloat(physicsImpostors[i].velocity.x), parseFloat(physicsImpostors[i].velocity.y), parseFloat(physicsImpostors[i].velocity.z)));
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
        var start = new BABYLON.Vector3(-10, -1, -10);
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
    createEmptyScene(canvas, engine) {
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

        var light = new BABYLON.HemisphericLight(
            "light",
            new BABYLON.Vector3(0, 1, 0),
            scene
        );
        light.intensity = 0.7;

        store.commit("experiment/SET_LIGHT", light);

        var gravityVector = new BABYLON.Vector3(0, -9.81, 0);
        var physicsPlugin = new BABYLON.CannonJSPlugin();
        scene.enablePhysics(gravityVector, physicsPlugin);

        var ground = BABYLON.MeshBuilder.CreateBox("ground", {
            width: 20,
            height: 1,
            depth: 20
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

        var start = new BABYLON.Vector3(-10, -1, -10);
        var axis = this.createAxis(start, 5, scene);
        store.commit("experiment/SET_AXIS", axis);

        store.commit("experiment/SET_PLAYING", true);

        return scene;
    }
}