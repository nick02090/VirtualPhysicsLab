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

            <b-table :data="log" hoverable paginated :per-page="5">
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
                            <p>Ne postoji povijest nadzora odabranog elementa.</p>
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
            mesh: null
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
            return this.getMeshLog(this.mesh);
        }
    },
    methods: {
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
                babylon.toggleHighlight(this.getMeshByName(previousMesh));
            }
            babylon.toggleHighlight(this.getMeshByName(newMesh));
        }
    }
};
</script>

<style scoped>
</style>

