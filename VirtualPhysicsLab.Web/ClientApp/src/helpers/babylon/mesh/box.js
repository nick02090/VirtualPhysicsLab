export default {
    create(properties, material, scene) {
        var box = BABYLON.MeshBuilder.CreateBox(
            properties.name, {
                width: 2,
                height: 2,
                depth: 2
            },
            scene
        );
        box.position.x = 0;
        box.position.y = 0;
        box.position.z = 0;
        box.physicsImpostor = new BABYLON.PhysicsImpostor(
            box,
            BABYLON.PhysicsImpostor.BoxImpostor, {
                mass: parseFloat(properties.mass)
            },
            scene
        );
        box.material = material;
        return box;
    }
}