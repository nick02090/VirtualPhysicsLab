<template>
    <div id="menu">
        <aside class="menu">
            <transition mode="out-in">
                <div v-if="isMenu">
                    <p class="menu-label">
                        <a class="button icon-btn" @click="changeMenu">
                            <span class="icon">
                                <i class="fas fa-ellipsis-h"></i>
                            </span>
                        </a>
                    </p>
                    <div v-for="(group, index) in menus" :key="index" class="menu-group">
                        <p class="menu-label">{{group.name}}</p>
                        <ul class="menu-list">
                            <li v-for="(menu, index) in group.menus" :key="index">
                                <a
                                    @click="changeMenu({'groupName': group.name, 'menuName': menu.name})"
                                    :class="{'is-active': activeMenu.name == menu.name}"
                                >{{menu.name}}</a>
                            </li>
                        </ul>
                    </div>
                </div>
                <p v-else>
                    <span>
                        <div class="is-flex">
                            <p class="menu-label">
                                <a class="button icon-btn" @click="changeMenu">
                                    <span class="icon">
                                        <i class="fas fa-ellipsis-v"></i>
                                    </span>
                                </a>
                            </p>
                            <p class="menu-label">{{activeMenu.name}}</p>
                        </div>
                        <component
                            :is="activeMenu.component"
                            @changeMenu="changeMenu"
                            @notification="notification"
                            @error="error"
                        ></component>
                    </span>
                </p>
            </transition>
        </aside>
    </div>
</template>

<script>
import NoviElement from "@/components/experiment/menus/NoviElement.vue";
import UrediElement from "@/components/experiment/menus/UrediElement.vue";
import RadnoOkruzenje from "@/components/experiment/menus/RadnoOkruzenje.vue";
import FizikaElementa from "@/components/experiment/menus/FizikaElementa.vue";
import Statistika from "@/components/experiment/menus/Statistika.vue";
import { mapGetters, mapState } from "vuex";
import babylon from "@/helpers/babylon/babylon.js";

export default {
    name: "Menu",
    data() {
        return {
            isMenu: true,
            secondsCounter: 1,
            activeMenu: {
                name: "Novi element",
                component: NoviElement
            },
            menus: [
                {
                    name: "Općenito",
                    menus: [
                        {
                            name: "Novi element",
                            component: NoviElement
                        },
                        {
                            name: "Uredi element",
                            component: UrediElement
                        },
                        {
                            name: "Radno okruženje",
                            component: RadnoOkruzenje
                        }
                    ]
                },
                {
                    name: "Praćenje promjena",
                    menus: [
                        {
                            name: "Fizika elementa",
                            component: FizikaElementa
                        },
                        {
                            name: "Statistika",
                            component: Statistika
                        }
                    ]
                }
            ]
        };
    },
    components: {
        NoviElement,
        UrediElement,
        RadnoOkruzenje,
        FizikaElementa,
        Statistika
    },
    mounted() {
        setInterval(() => {
            this.updateLogs();
        }, 1);
    },
    computed: {
        ...mapState({
            deletedMesh: state => state.experiment.deletedMesh,
            existingMeshes: state => state.experiment.meshes
        }),
        ...mapGetters({
            getMeshByName: "experiment/getMeshByName",
            getMeshLog: "experiment/getMeshLog"
        })
    },
    methods: {
        changeMenu(menuData) {
            this.isMenu = !this.isMenu;
            if (menuData.groupName && menuData.menuName) {
                var group = this.menus.find(x => x.name === menuData.groupName);
                this.activeMenu = group.menus.find(
                    x => x.name == menuData.menuName
                );
                this.isMenu = false;
            }
        },
        updateLogs() {
            for (var i in this.existingMeshes) {
                var mesh = this.getMeshByName(this.existingMeshes[i]);
                var name = this.existingMeshes[i];
                var physicsImpostor = mesh.physicsImpostor;
                if (physicsImpostor.isDisposed === false) {
                    this.updatePhysics(mesh, name);
                    this.updateCollisions(mesh, name);
                    if (this.secondsCounter === 500) {
                        this.updateRotation(mesh, name);
                    }
                    this.updateMovement(mesh, name);
                    this.updateForces(mesh, name);
                }
            }
            this.secondsCounter += 1;
            if (this.secondsCounter === 1001) {
                this.secondsCounter = 1;
            }
        },
        updatePhysics(mesh, name) {},
        updateCollisions(mesh, name) {
            var collisions = babylon.getCollisions(mesh, name);
            var logs = this.getMeshLog(name).filter(
                x => x.type === "collisions"
            );
            for (var i in this.existingMeshes) {
                var currentMesh = this.existingMeshes[i];
                if (currentMesh === name) continue;
                var log = logs.find(
                    x =>
                        x.properties.collider === currentMesh &&
                        x.properties.time.end === null
                );
                var collider = collisions.find(x => x === currentMesh);
                if (log === undefined && collider !== undefined) {
                    var firstTime = Date.now();
                    var meshLog = {
                        mesh: name,
                        log: {
                            name: "Sudar",
                            icon: "fas fa-bullseye",
                            type: "collisions",
                            description: `(${collider})`,
                            key: firstTime,
                            properties: {
                                collider: collider,
                                time: {
                                    start: firstTime,
                                    end: null
                                }
                            }
                        }
                    };
                    this.$store.commit("experiment/SET_MESH_LOGS", meshLog);
                } else if (log !== undefined && collider === undefined) {
                    var startTime = log.properties.time.start;
                    var time = Date.now();
                    var meshLog = {
                        mesh: name,
                        log: {
                            name: log.name,
                            icon: log.icon,
                            type: log.type,
                            description: log.description,
                            key: log.key,
                            properties: {
                                collider: log.properties.collider,
                                time: {
                                    start: startTime,
                                    end: time
                                }
                            }
                        }
                    };
                    this.$store.commit("experiment/UPDATE_MESH_LOGS", meshLog);
                }
            }
        },
        updateRotation(mesh, name) {
            var eulerAngles = mesh.rotationQuaternion.toEulerAngles();
            var rotation = {
                x: this.radianToDegree(eulerAngles.x),
                y: this.radianToDegree(eulerAngles.y),
                z: this.radianToDegree(eulerAngles.z)
            };
            var logs = this.getMeshLog(name).filter(x => x.type === "rotation");
            var size = logs.length;
            if (size > 0) {
                var logX = logs.find(x => x.key === "x");
                this.compareRotation(rotation, "x", name, logX);

                var logY = logs.find(x => x.key === "y");
                this.compareRotation(rotation, "y", name, logY);

                var logZ = logs.find(x => x.key === "z");
                this.compareRotation(rotation, "z", name, logZ);
            } else {
                this.compareRotation(rotation, "x", name, undefined);
                this.compareRotation(rotation, "y", name, undefined);
                this.compareRotation(rotation, "z", name, undefined);
            }
        },
        compareRotation(rotation, axis, name, log) {
            if (log === undefined) {
                var meshLog = {
                    mesh: name,
                    log: {
                        name: "Rotacija",
                        icon: "fas fa-sync-alt",
                        type: "rotation",
                        description: `(${axis}-os)`,
                        key: axis,
                        properties: {
                            rotations: [
                                {
                                    value: rotation,
                                    time: Date.now()
                                }
                            ]
                        }
                    }
                };
                this.$store.commit("experiment/SET_MESH_LOGS", meshLog);
            } else {
                var size = log.properties.rotations.length;
                var lastRotation = log.properties.rotations[size - 1].value;
                if (
                    Math.abs(rotation[axis] - lastRotation[axis]).toFixed(2) > 0
                ) {
                    var newRotations = log.properties.rotations;
                    newRotations.push({
                        value: rotation,
                        time: Date.now()
                    });
                    var meshLog = {
                        mesh: name,
                        log: {
                            name: "Rotacija",
                            icon: "fas fa-sync-alt",
                            type: "rotation",
                            description: `(${axis}-os)`,
                            key: axis,
                            properties: {
                                rotations: newRotations
                            }
                        }
                    };
                    this.$store.commit("experiment/UPDATE_MESH_LOGS", meshLog);
                }
            }
        },
        updateMovement(mesh) {},
        updateForces(mesh) {},
        radianToDegree(degrees) {
            var pi = Math.PI;
            var radians = degrees * (180 / pi);
            return parseFloat(radians.toFixed(2));
        },
        notification(msg) {
            this.$emit("notification", msg);
        },
        error(msg) {
            this.$emit("error", msg);
        }
    },
    watch: {
        deletedMesh(current, previous) {
            if (previous == null && current != null) {
                this.error(`Izbrisan je element ${current}`);
                this.$store.commit("experiment/SET_DELETED_MESH", null);
            }
        }
    }
};
</script>

<style scoped>
.menu-group {
    padding-bottom: 10px;
}
aside {
    margin-left: 10px;
}
.v-leave {
    opacity: 1;
}
.v-leave-active {
    transition: opacity 0.25s;
}
.v-leave-to {
    opacity: 0;
}
.v-enter {
    opacity: 0;
}
.v-enter-active {
    transition: opacity 0.5s;
}
.v-enter-to {
    opacity: 1;
}
</style>
