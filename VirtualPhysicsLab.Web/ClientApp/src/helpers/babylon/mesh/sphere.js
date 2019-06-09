export default {
    create(properties, material, scene) {
        var sphere = BABYLON.MeshBuilder.CreateSphere(
            properties.name, {
                diameterX: 1,
                diameterY: 1,
                diameterZ: 1
            },
            scene
        );
        sphere.position.x = 0;
        sphere.position.y = -0.5;
        sphere.position.z = 0;
        sphere.physicsImpostor = new BABYLON.PhysicsImpostor(
            sphere,
            BABYLON.PhysicsImpostor.SphereImpostor, {
                mass: parseFloat(properties.mass),
                friction: parseFloat(properties.friction),
                restitution: parseFloat(properties.restitution)
            },
            scene
        );
        sphere.material = material;
        return sphere;
    }
}