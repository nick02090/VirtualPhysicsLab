<template>
    <div id="fizika-elementa">
        <div class="field" v-if="currentMesh == null">
            <p>Trenutno ne postoje elementi na sceni.</p>
            <button class="button is-success" @click="changeMenu('Općenito', 'Novi element')">
                <span class="icon">
                    <i class="fas fa-plus"></i>
                </span>
                <span>Dodaj</span>
            </button>
        </div>
        <div class="field" v-else>
            <b-field horizontal label="Element:">
                <div class="select">
                    <select v-model="mesh">
                        <option v-for="mesh in existingMeshes" :key="mesh" :value="mesh">{{mesh}}</option>
                    </select>
                </div>
            </b-field>
            <b-table :data="physic" hoverable paginated :per-page="5" :pagination-simple="true">
                <template slot="header">Fizička svojstva</template>

                <template slot-scope="props">
                    <b-table-column field="name">
                        <span class="icon">
                            <i :class="props.row.icon"></i>
                        </span>
                        {{ props.row.name }}
                        ({{ physicInfo(props.row) }})
                        {{ props.row.description }}
                    </b-table-column>
                </template>

                <template slot="empty">
                    <section class="section">
                        <div class="content has-text-grey has-text-centered">
                            <p>Ne postoji fizika odabranog elementa.</p>
                        </div>
                    </section>
                </template>

                <template slot="footer">
                    <button class="button is-link is-fullwidth is-outlined" @click="createPhysic">
                        <span class="icon">
                            <i class="fas fa-plus"></i>
                        </span>
                        <span>Dodaj</span>
                    </button>
                </template>
            </b-table>

            <b-modal v-if="selectedPhysic" :active.sync="isModalActive" scroll="keep" width="350px">
                <header class="modal-card-head">
                    <p class="modal-card-title">Fizika</p>
                    <div class="select">
                        <b-select v-model="selectedPhysic" :icon="selectedPhysic.icon">
                            <option
                                v-for="physic in physics"
                                :key="physic.name"
                                :value="physic"
                            >{{ physic.name }}</option>
                        </b-select>
                    </div>
                </header>
                <section class="modal-card-body">
                    <component
                        :is="selectedPhysic.component"
                        ref="physic"
                        @addPhysic="updatePhysics"
                    ></component>
                </section>
                <footer class="modal-card-foot">
                    <button class="button" type="button" @click="isModalActive = false">Odustani</button>
                    <button class="button is-success" @click="addPhysic">
                        <span class="icon">
                            <i class="fas fa-plus"></i>
                        </span>
                        <span>Dodaj</span>
                    </button>
                </footer>
            </b-modal>
        </div>
    </div>
</template>

<script>
import babylon from "@/helpers/babylon/babylon.js";
import colors from "@/helpers/colors.js";
import * as meshTypes from "@/helpers/babylon/mesh/mesh-types.js";
import { mapGetters, mapState } from "vuex";
import Brzina from "@/components/experiment/physics/Brzina.vue";
import { setInterval } from "timers";

export default {
    name: "FizikaElementa",
    data() {
        return {
            mesh: null,
            isModalActive: false,
            selectedPhysic: null,
            meshPhysic: {
                velocity: {
                    x: 0,
                    y: 0,
                    z: 0
                }
            },
            physics: [
                {
                    name: "Brzina",
                    icon: "fas fa-running",
                    component: Brzina
                }
            ]
        };
    },
    components: {
        Brzina
    },
    beforeDestroy() {
        if (this.mesh) {
            babylon.toggleHighlight(this.getMeshByName(this.mesh));
        }
    },
    computed: {
        ...mapGetters({
            getMeshByName: "experiment/getMeshByName",
            getMeshPhysic: "experiment/getMeshPhysic",
            getMeshLog: "experiment/getMeshLog"
        }),
        ...mapState({
            existingMeshes: state => state.experiment.meshes
        }),
        scene: {
            get: function() {
                return this.$store.state.experiment.scene;
            },
            set: function(data) {
                this.$store.commit("experiment/SET_SCENE", data);
            }
        },
        physic() {
            return this.getMeshPhysic(this.mesh);
        },
        currentMesh: {
            get: function() {
                if (this.existingMeshes.length === 0) {
                    this.mesh = null;
                    return null;
                }
                if (this.mesh == null) {
                    var size = this.existingMeshes.length;
                    if (size > 0) {
                        this.mesh = this.existingMeshes[size - 1];
                        return this.mesh;
                    }
                    return null;
                } else {
                    return this.mesh;
                }
            },
            set: function(mesh) {
                this.mesh = mesh;
            }
        }
    },
    methods: {
        physicInfo(physic) {
            switch (physic.name) {
                case "Brzina":
                    return `${physic.properties.value} ${
                        physic.properties.unit
                    }`;
                    break;
                default:
                    break;
            }
        },
        createPhysic() {
            this.selectedPhysic = this.physics[0];
            this.isModalActive = true;
        },
        async addPhysic() {
            await this.$refs["physic"].addPhysic();
        },
        updatePhysics(physic) {
            var meshPhysic = {
                mesh: this.mesh,
                physic: physic
            };
            this.$store.commit("experiment/UPDATE_MESH_PHYSICS", meshPhysic);
            var values = [];
            let startTime = Date.now();
            switch (physic.properties.axisString) {
                case "x":
                    values.push({
                        velocity: {
                            x: physic.properties.value,
                            y: 0,
                            z: 0
                        },
                        time: startTime
                    });
                    break;
                case "y":
                    values.push({
                        velocity: {
                            x: 0,
                            y: physic.properties.value,
                            z: 0
                        },
                        time: startTime
                    });
                    break;
                case "z":
                    values.push({
                        velocity: {
                            x: 0,
                            y: physic.properties.value,
                            z: 0
                        },
                        time: startTime
                    });
                    break;
                default:
                    break;
            }
            var meshLog = {
                mesh: this.mesh,
                log: {
                    name: "Dodana fizika",
                    icon: "fas fa-atom",
                    type: "physics",
                    key: "velocity",
                    description: `(${physic.name})`,
                    properties: {
                        axis: physic.properties.axis,
                        axisString: physic.properties.axisString,
                        values: values,
                        time: {
                            start: Date.now(),
                            end: null
                        }
                    }
                }
            };
            var logs = this.getMeshLog(this.mesh).filter(
                x => x.type === "physics" && x.key === "velocity"
            );
            var log = logs.find(
                x =>
                    x.properties.axis === physic.properties.axis &&
                    x.properties.time.end === null
            );
            if (log === undefined) {
                this.$store.commit("experiment/SET_MESH_LOGS", meshLog);
            }
            this.isModalActive = false;
            babylon.addPhysic(this.getMeshByName(this.mesh), physic);
        },
        notification(msg) {
            this.$emit("notification", msg);
        },
        error(msg) {
            this.$emit("error", msg);
        },
        changeMenu(groupName, menuName) {
            this.$emit("changeMenu", {
                groupName: groupName,
                menuName: menuName
            });
        }
    },
    watch: {
        mesh(newMesh, previousMesh) {
            if (previousMesh) {
                if (this.existingMeshes.length > 0) {
                    if (this.existingMeshes.includes(previousMesh)) {
                        babylon.toggleHighlight(
                            this.getMeshByName(previousMesh)
                        );
                    }
                }
            }
            if (newMesh) {
                babylon.toggleHighlight(this.getMeshByName(newMesh));
            }
        },
        existingMeshes(newMeshes, previousMeshes) {
            if (newMeshes.length > 0) {
                if (newMeshes.includes(this.mesh) === false) {
                    let size = newMeshes.length;
                    this.mesh = newMeshes[size - 1];
                }
            }
        }
    }
};
</script>

<style scoped>
</style>
