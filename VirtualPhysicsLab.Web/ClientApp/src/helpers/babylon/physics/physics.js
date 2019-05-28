import store from "@/store/index.js"

export default {
    addPhysic(obj, physic) {
        switch (physic.name) {
            case "Brzina":
                this.addVelocity(obj, physic.properties)
                break;
            default:
                break;
        }
    },
    addVelocity(mesh, properties) {
        var isPlaying = store.state.experiment.playing;
        if (isPlaying === true) {
            switch (properties.axis) {
                case 0:
                    mesh.physicsImpostor.setLinearVelocity(new BABYLON.Vector3(parseFloat(properties.value), 0, 0));
                    break;
                case 2:
                    mesh.physicsImpostor.setLinearVelocity(new BABYLON.Vector3(0, 0, parseFloat(properties.value)));
                    break;
            }
        } else {
            var physicsImpostors = store.state.experiment.physicsImpostors;
            var physicsImpostor = physicsImpostors.find(x => x.name === mesh.name);
            var newPhysicsImpostor = {
                ...physicsImpostor
            };
            switch (properties.axis) {
                case 0:
                    newPhysicsImpostor.velocity.x = parseFloat(properties.value);
                    break;
                case 2:
                    newPhysicsImpostor.velocity.z = parseFloat(properties.value);
                    break;
            }
            var arrayRemove = (arr, value) => {
                return arr.filter(function (ele) {
                    return ele != value;
                });
            }
            physicsImpostors = arrayRemove(physicsImpostors, physicsImpostor);
            physicsImpostors.push(newPhysicsImpostor);
            store.commit("experiment/SET_MESH_IMPOSTORS", physicsImpostors);
        }
    }
}