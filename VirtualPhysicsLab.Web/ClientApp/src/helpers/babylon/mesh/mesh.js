import * as meshTypes from "./mesh-types"
import box from "./box";
import store from "@/store/index.js"

export default {
    create(type, properties, material, scene) {
        switch (type) {
            case meshTypes.BOX:
                return box.create(properties, material, scene);
            default:
                console.error("Invalid type of mesh!");
                return;
        }
    },
    toggleHightlight(mesh) {
        var hl = store.state.experiment.highlight;
        if (hl.hasMesh(mesh)) {
            hl.removeMesh(mesh);
        } else {
            hl.addMesh(mesh, BABYLON.Color3.Green());
        }
    }
}