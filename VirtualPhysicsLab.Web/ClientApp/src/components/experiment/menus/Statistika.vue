<template>
    <div id="statistika">
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

            <b-field horizontal label="Praćenja">
                <b-tooltip
                    label="Uključi/Isključi sva praćenja."
                    type="is-black"
                    position="is-right"
                >
                    <b-switch @input="globalSelection" v-model="all"></b-switch>
                </b-tooltip>
            </b-field>

            <b-field grouped group-multiline>
                <div class="control" v-for="(log, index) in logs" :key="index">
                    <b-tooltip :label="log.description" type="is-black" position="is-right">
                        <b-checkbox v-model="log.visible">{{log.name}}</b-checkbox>
                    </b-tooltip>
                </div>
            </b-field>

            <b-table :data="log" hoverable paginated :per-page="5" :pagination-simple="true">
                <template slot="header">Povijest nadzora</template>

                <template slot-scope="props">
                    <b-table-column field="name">
                        <b-tooltip label="Detalji" type="is-black">
                            <a @click="logInfo">
                                <span class="icon">
                                    <i :class="props.row.icon"></i>
                                </span>
                                {{ props.row.name }}
                                {{ props.row.description }}
                            </a>
                        </b-tooltip>
                    </b-table-column>
                </template>

                <template slot="empty">
                    <section class="section">
                        <div class="content has-text-grey has-text-centered">
                            <p>Ne postoji povijest nadzora odabranog elementa uz odabrane filtere.</p>
                        </div>
                    </section>
                </template>
            </b-table>
        </div>
    </div>
</template>

<script>
import babylon from "@/helpers/babylon/babylon.js";
import colors from "@/helpers/colors.js";
import * as meshTypes from "@/helpers/babylon/mesh/mesh-types.js";
import { mapGetters, mapState } from "vuex";
import { setInterval } from "timers";

export default {
    name: "Statistika",
    data() {
        return {
            mesh: null,
            all: false,
            logs: [
                {
                    name: "Fizika",
                    type: "physics",
                    description: "Fizička svojstva elementa.",
                    visible: false
                },
                {
                    name: "Sudari",
                    type: "collisions",
                    description: "Interakcija s drugim elementima.",
                    visible: false
                },
                {
                    name: "Rotacija",
                    type: "rotation",
                    description: "Promjene smjera elementa.",
                    visible: false
                },
                {
                    name: "Pomak",
                    type: "movement",
                    description: "Kretnja elementa po sceni.",
                    visible: false
                }
            ]
        };
    },
    beforeDestroy() {
        if (this.mesh) {
            babylon.toggleHighlight(this.getMeshByName(this.mesh));
        }
    },
    computed: {
        ...mapGetters({
            getMeshByName: "experiment/getMeshByName",
            getMeshLog: "experiment/getMeshLog"
        }),
        ...mapState({
            existingMeshes: state => state.experiment.meshes
        }),
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
        },
        log() {
            var allLogs = this.getMeshLog(this.mesh);
            var filtered = [];
            var selected = this.logs.filter(x => x.visible);
            for (var i in allLogs) {
                for (var j in selected) {
                    if (allLogs[i].type === selected[j].type) {
                        filtered.push(allLogs[i]);
                    }
                }
            }
            if (selected.length < this.logs.length) {
                this.all = false;
            } else if (selected.length == this.logs.length) {
                this.all = true;
            }
            return filtered;
        }
    },
    methods: {
        globalSelection(value) {
            for (var i in this.logs) {
                this.logs[i].visible = value;
            }
        },
        logInfo() {
            console.log("will open modal with details");
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

