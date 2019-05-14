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
                    this.updateRotation(mesh, name);
                    this.updateMovement(mesh, name);
                    this.updateForces(mesh, name);
                }
            }
        },
        updatePhysics(mesh, name) {},
        updateCollisions(mesh, name) {
            var collisions = babylon.getCollisions(mesh, name);
            var logs = this.getMeshLog(name).filter(
                x => x.type === "collisions"
            );
            for (var i in collisions) {
                var log = logs.find(
                    x => x.properties.collider === collisions[i]
                );
                if (log === undefined) {
                    var firstTime = Date.now();
                    var meshLog = {
                        mesh: name,
                        log: {
                            name: "Sudar",
                            icon: "fas fa-bullseye",
                            type: "collisions",
                            description: `(${collisions[i]})`,
                            key: collisions[i],
                            properties: {
                                collider: collisions[i],
                                counter: 1,
                                times: [firstTime]
                            }
                        }
                    };
                    this.$store.commit("experiment/SET_MESH_LOGS", meshLog);
                } else {
                    var newTimes = log.properties.times;
                    newTimes.push(Date.now());
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
                                counter: log.properties.counter + 1,
                                times: newTimes
                            }
                        }
                    };
                    this.$store.commit("experiment/UPDATE_MESH_LOGS", meshLog);
                }
            }
        },
        updateRotation(mesh) {},
        updateMovement(mesh) {},
        updateForces(mesh) {},
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
