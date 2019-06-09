export default {
    create(properties, material, scene) {
        var box = BABYLON.MeshBuilder.CreateBox(
            properties.name, {
                width: 1,
                height: 1,
                depth: 1
            },
            scene
        );
        box.position.x = 0;
        box.position.y = -0.5;
        box.position.z = 0;
        box.physicsImpostor = new BABYLON.PhysicsImpostor(
            box,
            BABYLON.PhysicsImpostor.BoxImpostor, {
                mass: parseFloat(properties.mass),
                friction: parseFloat(properties.friction),
                restitution: parseFloat(properties.restitution)
            },
            scene
        );
        box.material = material;
        return box;
    }
}