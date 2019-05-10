<template>
    <div id="uredi-element">
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
                    <select v-model="mesh" :disabled="isDrag">
                        <option v-for="mesh in existingMeshes" :key="mesh" :value="mesh">{{mesh}}</option>
                    </select>
                </div>
            </b-field>
            <div class="field">
                <label class="label">Postavke</label>
                <b-tooltip v-if="hasCollisions && isDrag" type="is-black" label="Postoje kolizije!">
                    <b-switch v-model="isDrag" disabled>Pomakni</b-switch>
                </b-tooltip>
                <b-switch v-else v-model="isDrag" @input="switchDrag">Pomakni</b-switch>
                <div class="block">
                    <b-radio v-model="axis" :native-value="0">x</b-radio>
                    <b-radio v-model="axis" :native-value="1">y</b-radio>
                    <b-radio v-model="axis" :native-value="2">z</b-radio>
                </div>
                <div class="field">
                    <label class="label">Rotacija (os Y)</label>
                    <b-field>
                        <b-field>
                            <b-tooltip label="Stupnjevi" type="is-black">
                                <b-input v-model="rotation.d"></b-input>
                            </b-tooltip>
                            <p class="control">
                                <span class="button is-static">°</span>
                            </p>
                        </b-field>
                        <b-field>
                            <b-tooltip label="Minute" type="is-black">
                                <b-input
                                    v-model="rotation.m"
                                    name="minutes"
                                    v-validate="'numeric|min_value:0|max_value:59'"
                                ></b-input>
                            </b-tooltip>
                            <p class="control">
                                <span class="button is-static">'</span>
                            </p>
                        </b-field>
                        <b-field>
                            <b-tooltip label="Sekunde" type="is-black">
                                <b-input
                                    v-model="rotation.s"
                                    name="seconds"
                                    v-validate="'numeric|min_value:0|max_value:59'"
                                ></b-input>
                            </b-tooltip>
                            <p class="control">
                                <span class="button is-static">"</span>
                            </p>
                        </b-field>
                        <b-tooltip label="Resetiraj" type="is-black">
                            <button class="button is-warning" @click="resetRotation">
                                <span class="icon">
                                    <i class="fas fa-redo"></i>
                                </span>
                            </button>
                        </b-tooltip>
                    </b-field>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import babylon from "@/helpers/babylon/babylon.js";
import colors from "@/helpers/colors.js";
import * as meshTypes from "@/helpers/babylon/mesh/mesh-types.js";
import { mapGetters, mapState } from "vuex";

export default {
    name: "UrediElement",
    data() {
        return {
            isDrag: false,
            hasCollisions: false,
            axis: 0,
            mesh: null,
            rotation: {
                d: 0,
                m: 0,
                s: 0
            }
        };
    },
    mounted() {
        setInterval(() => {
            this.setDMS();
        }, 1);
        setInterval(() => {
            this.checkCollisions();
        }, 1);
    },
    beforeDestroy() {
        if (this.isDrag) {
            if (this.hasCollisions) {
                babylon.setMeshToLastPosition(this.getMeshByName(this.mesh));
            }
            this.switchDrag();
        }
        if (this.mesh) {
            babylon.toggleHighlight(this.getMeshByName(this.mesh));
        }
    },
    computed: {
        ...mapGetters({
            getMeshByName: "experiment/getMeshByName"
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
        }
    },
    methods: {
        setCurrentRotation(rotation) {
            var mesh = this.getMeshByName(this.mesh);
            var axis = new BABYLON.Vector3(0, 1, 0);
            var angle = this.degreeToRadian(rotation);
            var quaternion = new BABYLON.Quaternion.RotationAxis(axis, angle);
            mesh.rotationQuaternion = quaternion;
            this.setDMS();
        },
        getCurrentRotation() {
            var mesh = this.getMeshByName(this.mesh);
            if (mesh == undefined) return undefined;
            return this.radianToDegree(
                mesh.rotationQuaternion.toEulerAngles().y
            );
        },
        setDMS() {
            var rotation = this.getCurrentRotation();
            if (rotation == undefined) return;
            var deg = rotation.toFixed(5);
            if (Math.abs(deg).toFixed(2) == 0) deg = Math.abs(deg);
            this.rotation.d = Math.floor(deg);
            var minFloat = (deg - this.rotation.d) * 60;
            this.rotation.m = Math.floor(minFloat);
            var secFloat = (minFloat - this.rotation.m) * 60;
            this.rotation.s = Math.floor(secFloat);
            if (this.rotation.s == 60) {
                this.rotation.s = 0;
                this.rotation.m++;
            }
            if (this.rotation.m == 60) {
                this.rotation.m = 0;
                this.rotation.d++;
            }
        },
        degreeToRadian(degrees) {
            var pi = Math.PI;
            return degrees * (pi / 180);
        },
        radianToDegree(degrees) {
            var pi = Math.PI;
            return degrees * (180 / pi);
        },
        checkCollisions() {
            if (this.isDrag) {
                this.hasCollisions = babylon.checkCollisions(
                    this.getMeshByName(this.mesh),
                    this.mesh
                );
            }
        },
        switchDrag(value) {
            var msg = `Fizika tijela (${this.mesh}) je isključena.`;
            if (!value) {
                babylon.removeDragBehaviours(
                    this.getMeshByName(this.mesh),
                    true
                );
                msg = `Fizika djeluje na tijelo (${this.mesh}).`;
            } else {
                babylon.addDragBehaviour(
                    this.getMeshByName(this.mesh),
                    this.axis,
                    true
                );
            }
            this.notification(msg);
        },
        setRotation() {
            var rotation =
                Number(this.rotation.d) +
                Number(this.rotation.m) / 60 +
                Number(this.rotation.s) / 3600;
            this.setCurrentRotation(rotation);
        },
        resetRotation() {
            this.setCurrentRotation(0);
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
        axis: function() {
            if (this.isDrag) {
                babylon.addDragBehaviour(
                    this.getMeshByName(this.mesh),
                    this.axis
                );
            }
        },
        "rotation.d": function() {
            this.setRotation();
        },
        "rotation.m": function() {
            this.setRotation();
        },
        "rotation.s": function() {
            this.setRotation();
        },
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
