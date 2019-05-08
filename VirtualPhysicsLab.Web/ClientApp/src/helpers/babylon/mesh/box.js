export default {
    create(properties, material, scene) {
        var box = BABYLON.MeshBuilder.CreateBox(
            properties.name, {
                width: properties.width,
                height: properties.height,
                depth: properties.depth
            },
            scene
        );
        box.position.x = properties.position.x;
        box.position.y = properties.position.y;
        box.position.z = properties.position.z;
        box.physicsImpostor = new BABYLON.PhysicsImpostor(
            box,
            BABYLON.PhysicsImpostor.BoxImpostor, {
                mass: properties.mass,
            },
            scene
        );
        box.material = material;
        return box;
    }
}