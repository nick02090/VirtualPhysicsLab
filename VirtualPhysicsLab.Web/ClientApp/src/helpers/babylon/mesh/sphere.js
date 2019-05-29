export default {
    create(properties, material, scene) {
        var sphere = BABYLON.MeshBuilder.CreateSphere(
            properties.name, {
                diameterX: 2,
                diameterY: 2,
                diameterZ: 2
            },
            scene
        );
        sphere.position.x = 0;
        sphere.position.y = 0;
        sphere.position.z = 0;
        sphere.physicsImpostor = new BABYLON.PhysicsImpostor(
            sphere,
            BABYLON.PhysicsImpostor.SphereImpostor, {
                mass: parseFloat(properties.mass),
                friction: parseFloat(properties.friction)
            },
            scene
        );
        sphere.material = material;
        return sphere;
    }
}