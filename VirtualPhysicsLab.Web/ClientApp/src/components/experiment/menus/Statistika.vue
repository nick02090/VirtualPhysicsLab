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
                            <a @click="logInfo(props.row)">
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

            <b-modal
                v-if="selectedStatistic"
                :active.sync="isModalActive"
                scroll="keep"
                @close="close"
            >
                <header class="modal-card-head">
                    <p class="modal-card-title">
                        {{selectedStatistic.name}} - {{mesh}}
                        <span v-if="time">({{time}})</span>
                    </p>
                </header>
                <section class="modal-card-body">
                    <b-loading :is-full-page="false" :active.sync="isLoading"></b-loading>
                    <component
                        :is="selectedStatistic.component"
                        ref="statistic"
                        :log="selectedLog"
                        :mesh="mesh"
                    ></component>
                </section>
                <footer class="modal-card-foot">
                    <button class="button" type="button" @click="isModalActive = false">
                        <span class="icon">
                            <i class="fas fa-angle-left"></i>
                        </span>
                        <span>Nazad</span>
                    </button>
                    <button
                        class="button is-success"
                        @click="updateStatistics"
                        :disabled="!isUpdateable"
                    >
                        <span class="icon">
                            <i class="fas fa-redo"></i>
                        </span>
                        <span>Ažuriraj</span>
                    </button>
                </footer>
            </b-modal>
        </div>
    </div>
</template>

<script>
import Fizika from "@/components/experiment/statistics/Fizika.vue";
import Pomak from "@/components/experiment/statistics/Pomak.vue";
import Rotacija from "@/components/experiment/statistics/Rotacija.vue";
import Sudari from "@/components/experiment/statistics/Sudari.vue";

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
            isModalActive: false,
            selectedStatistic: null,
            selectedLog: null,
            isUpdateable: false,
            time: null,
            isLoading: false,
            statistics: [
                {
                    name: "Fizika",
                    icon: "fas fa-atom",
                    component: Fizika,
                    type: "physics"
                },
                {
                    name: "Sudari",
                    icon: "fas fa-bullseye",
                    component: Sudari,
                    type: "collisions"
                },
                {
                    name: "Rotacija",
                    icon: "fas fa-sync-alt",
                    component: Rotacija,
                    type: "rotation"
                },
                {
                    name: "Pomak",
                    icon: "fas fa-shoe-prints",
                    component: Pomak,
                    type: "movement"
                }
            ],
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
    components: {
        Fizika,
        Pomak,
        Rotacija,
        Sudari
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
        close() {
            this.isModalActive = false;
            this.selectedStatistic = null;
        },
        updateStatistics() {
            this.isLoading = true;
            this.isUpdateable = false;
            var allLogs = this.getMeshLog(this.mesh);
            this.selectedLog = allLogs.find(
                x =>
                    x.type === this.selectedLog.type &&
                    x.key === this.selectedLog.key
            );
            this.calculateTime();
            setTimeout(() => {
                this.isLoading = false;
            }, 1000);
        },
        globalSelection(value) {
            for (var i in this.logs) {
                this.logs[i].visible = value;
            }
        },
        logInfo(log) {
            this.selectedStatistic = this.statistics.find(
                x => x.type === log.type
            );
            this.selectedLog = log;
            this.calculateTime();
            this.isModalActive = true;
        },
        calculateTime() {
            switch (this.selectedLog.type) {
                case "physics":
                    switch (this.selectedLog.key) {
                        case "velocity":
                            if (this.selectedLog.properties.time.end) {
                                var start = this.selectedLog.properties.time
                                    .start;
                                var end = this.selectedLog.properties.time.end;
                                var diff = end - start;
                                this.time = this.formatMSDiff(diff);
                                this.isUpdateable = false;
                            } else {
                                this.time = `U izvođenju...`;
                                this.isUpdateable = true;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "collisions":
                    this.time = null;
                    this.isUpdateable = false;
                    break;
                case "rotation":
                    this.time = null;
                    this.isUpdateable = true;
                    break;
                case "movement":
                    this.time = null;
                    this.isUpdateable = true;
                    break;
                default:
                    break;
            }
        },
        formatMSDiff(diff) {
            var sec = diff / 1000;
            if (sec > 60) {
                var min = sec / 60;
                if (min > 60) {
                    var hours = min / 60;
                    return `${hours.toFixed(2)} h`;
                } else {
                    var secString = (min - Math.trunc(min)) * 60;
                    return `${Math.trunc(min)} min ${Math.round(secString)} s`;
                }
            } else {
                return `${sec.toFixed(2)} s`;
            }
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

