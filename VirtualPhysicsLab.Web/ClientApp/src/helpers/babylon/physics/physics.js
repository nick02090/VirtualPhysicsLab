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
        switch (properties.axis) {
            case 0:
                mesh.physicsImpostor.setLinearVelocity(new BABYLON.Vector3(parseFloat(properties.value), 0, 0));
                break;
            case 2:
                mesh.physicsImpostor.setLinearVelocity(new BABYLON.Vector3(0, 0, parseFloat(properties.value)));
                break;
        }
    }
}