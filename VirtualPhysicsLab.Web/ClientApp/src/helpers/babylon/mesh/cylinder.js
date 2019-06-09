export default {
    create(properties, material, scene) {
        var cylinder = BABYLON.MeshBuilder.CreateCylinder(
            properties.name, {
                height: 1,
                diameter: 1
            },
            scene
        );
        cylinder.position.x = 0;
        cylinder.position.y = -0.5;
        cylinder.position.z = 0;
        cylinder.physicsImpostor = new BABYLON.PhysicsImpostor(
            cylinder,
            BABYLON.PhysicsImpostor.CylinderImpostor, {
                mass: parseFloat(properties.mass),
                friction: parseFloat(properties.friction),
                restitution: parseFloat(properties.restitution)
            },
            scene
        );
        cylinder.material = material;
        return cylinder;
    }
}