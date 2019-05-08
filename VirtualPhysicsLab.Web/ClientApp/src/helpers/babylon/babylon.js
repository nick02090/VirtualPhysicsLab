import mesh from "./mesh/mesh"
import store from "@/store/index.js"
import physics from "./physics/physics"

export default {
    toggleHighlight(obj) {
        mesh.toggleHightlight(obj);
    },
    addDragBehaviour(obj, axis) {
        var canvas = store.state.experiment.canvas;
        var camera = store.state.experiment.camera;
        camera.detachControl(canvas);

        var dragBehaviours = store.state.experiment.dragBehaviour;
        this.removeDragBehaviours(obj);
        var dragBehaviour = dragBehaviours[axis];
        obj.addBehavior(dragBehaviour);
    },
    removeDragBehaviours(obj, permanent) {
        var dragBehaviours = store.state.experiment.dragBehaviour;
        dragBehaviours.forEach(dragBehaviour => {
            obj.removeBehavior(dragBehaviour);
        })

        if (permanent === true) {
            var canvas = store.state.experiment.canvas;
            var camera = store.state.experiment.camera;
            camera.attachControl(canvas, true);
        }
    },
    addMesh(type, properties, material, scene) {
        var newMesh = mesh.create(type, properties, material, scene);
        this.addMeshLabel(newMesh);
        this.enableWallRestrictions(newMesh);
        var physic = {
            name: properties.name,
            physic: []
        }
        store.commit("experiment/UPDATE_PHYSICS", physic);
        store.commit("experiment/UPDATE_MESHES", newMesh.name);
        return newMesh;
    },
    addPhysic(obj, physic) {
        physics.addPhysic(obj, physic);
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

        var floor = 0;

        scene.registerBeforeRender(function () {
            if (mesh.position.x > north) {
                mesh.position.x = north;
                mesh.position.y = floor;
            }
            if (mesh.position.x < south) {
                mesh.position.x = south;
                mesh.position.y = floor;
            }
            if (mesh.position.z > east) {
                mesh.position.z = east;
                mesh.position.y = floor;
            }
            if (mesh.position.z < west) {
                mesh.position.z = west;
                mesh.position.y = floor;
            }
        })
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
    createEmptyScene(canvas, engine) {
        var scene = new BABYLON.Scene(engine);

        var camera = new BABYLON.ArcRotateCamera(
            "camera",
            Math.PI / 2,
            Math.PI / 4,
            3,
            BABYLON.Vector3.Zero(),
            scene
        );
        camera.attachControl(canvas, true);
        camera.setPosition(new BABYLON.Vector3(15, 20, 15));
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
        var physicsPlugin = new BABYLON.OimoJSPlugin();
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
                restitution: 0
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

        return scene;
    }
}