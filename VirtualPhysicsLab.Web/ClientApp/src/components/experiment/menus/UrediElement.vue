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
                    <select v-model="mesh" :disabled="isDrag || isScale">
                        <option v-for="mesh in existingMeshes" :key="mesh" :value="mesh">{{mesh}}</option>
                    </select>
                </div>
            </b-field>

            <nav class="breadcrumb is-centered has-bullet-separator" aria-label="breadcrumbs">
                <ul>
                    <li :class="{'is-active': group == 0}">
                        <a @click="switchGroup(0)">
                            <span class="icon is-small">
                                <i class="fas fa-home" aria-hidden="true"></i>
                            </span>
                            <span>Općenito</span>
                        </a>
                    </li>
                    <li :class="{'is-active': group == 1}">
                        <a @click="switchGroup(1)">
                            <span class="icon is-small">
                                <i class="fas fa-crosshairs" aria-hidden="true"></i>
                            </span>
                            <span>Položaj</span>
                        </a>
                    </li>
                </ul>
            </nav>

            <div v-if="group == 0">
                <div class="control is-flex">
                    <div class="control margin-right">
                        <label class="label">Koordinatne osi</label>
                    </div>
                    <div class="control margin-left">
                        <b-tooltip
                            type="is-black"
                            label="Uključi/Isključi prikaz lokalnih koordinatnih osi."
                        >
                            <b-switch v-model="hasAxis" @input="switchAxis"></b-switch>
                        </b-tooltip>
                    </div>
                </div>
                <div class="control is-flex">
                    <div class="control margin-right">
                        <label class="label">Veličina</label>
                    </div>
                    <div class="control margin-left">
                        <b-tooltip
                            type="is-black"
                            label="Uključi/Isključi podešavanje veličine elementa."
                        >
                            <b-switch
                                v-model="isScale"
                                @input="switchScale"
                                :disabled="isDrag || !isPlaying"
                            ></b-switch>
                        </b-tooltip>
                    </div>
                </div>
                <b-field horizontal label="x-os">
                    <b-numberinput
                        :step="0.1"
                        v-model="size.x"
                        :min="0.1"
                        :max="5"
                        controls-position="compact"
                        controls-rounded
                        type="is-success"
                        :editable="false"
                        expanded
                        :disabled="!isScale"
                    ></b-numberinput>
                </b-field>
                <b-field horizontal label="y-os">
                    <b-numberinput
                        :step="0.1"
                        v-model="size.y"
                        :min="0.1"
                        :max="5"
                        controls-position="compact"
                        controls-rounded
                        type="is-success"
                        :editable="false"
                        expanded
                        :disabled="!isScale"
                    ></b-numberinput>
                </b-field>
                <b-field horizontal label="z-os">
                    <b-numberinput
                        :step="0.1"
                        v-model="size.z"
                        :min="0.1"
                        :max="5"
                        controls-position="compact"
                        controls-rounded
                        type="is-success"
                        :editable="false"
                        expanded
                        :disabled="!isScale"
                    ></b-numberinput>
                </b-field>
                <div class="field">
                    <button
                        class="button is-danger"
                        @click="deleteMesh"
                        :disabled="isDrag || !isPlaying || isScale"
                    >
                        <span class="icon">
                            <i class="fas fa-times"></i>
                        </span>
                        <span>Izbriši</span>
                    </button>
                </div>
            </div>

            <div v-if="group == 1">
                <div class="control is-flex">
                    <div class="control margin-right">
                        <label class="label">Pomakni</label>
                    </div>
                    <div class="control margin-left" style="text-align: right">
                        <b-tooltip
                            v-if="hasCollisions && isDrag"
                            type="is-black"
                            label="Postoje kolizije!"
                        >
                            <b-switch v-model="isDrag" disabled></b-switch>
                        </b-tooltip>
                        <b-switch v-else v-model="isDrag" @input="switchDrag"></b-switch>

                        <div class="block">
                            <b-radio v-model="axis" :native-value="0" :disabled="!isDrag">x</b-radio>
                            <b-radio v-model="axis" :native-value="1" :disabled="!isDrag">y</b-radio>
                            <b-radio v-model="axis" :native-value="2" :disabled="!isDrag">z</b-radio>
                        </div>
                    </div>
                </div>
                <div class="field">
                    <label class="label">Rotacija (os X)</label>
                    <b-field>
                        <b-field>
                            <b-tooltip label="Stupnjevi" type="is-black">
                                <b-input
                                    v-model="rotation.x.d"
                                    type="number"
                                    @input="setRotation(0)"
                                ></b-input>
                            </b-tooltip>
                            <p class="control">
                                <span class="button is-static">°</span>
                            </p>
                        </b-field>
                        <b-field>
                            <b-tooltip label="Minute" type="is-black">
                                <b-input
                                    v-model="rotation.x.m"
                                    name="minutes"
                                    type="number"
                                    @input="setRotation(0)"
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
                                    v-model="rotation.x.s"
                                    name="seconds"
                                    type="number"
                                    @input="setRotation(0)"
                                    v-validate="'numeric|min_value:0|max_value:59'"
                                ></b-input>
                            </b-tooltip>
                            <p class="control">
                                <span class="button is-static">"</span>
                            </p>
                        </b-field>
                        <b-tooltip label="Resetiraj" type="is-black">
                            <button class="button is-warning" @click="resetRotation(0)">
                                <span class="icon">
                                    <i class="fas fa-redo"></i>
                                </span>
                            </button>
                        </b-tooltip>
                    </b-field>
                </div>
                <div class="field">
                    <label class="label">Rotacija (os Y)</label>
                    <b-field>
                        <b-field>
                            <b-tooltip label="Stupnjevi" type="is-black">
                                <b-input
                                    v-model="rotation.y.d"
                                    type="number"
                                    @input="setRotation(1)"
                                ></b-input>
                            </b-tooltip>
                            <p class="control">
                                <span class="button is-static">°</span>
                            </p>
                        </b-field>
                        <b-field>
                            <b-tooltip label="Minute" type="is-black">
                                <b-input
                                    v-model="rotation.y.m"
                                    name="minutes"
                                    type="number"
                                    @input="setRotation(1)"
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
                                    v-model="rotation.y.s"
                                    name="seconds"
                                    type="number"
                                    @input="setRotation(1)"
                                    v-validate="'numeric|min_value:0|max_value:59'"
                                ></b-input>
                            </b-tooltip>
                            <p class="control">
                                <span class="button is-static">"</span>
                            </p>
                        </b-field>
                        <b-tooltip label="Resetiraj" type="is-black">
                            <button class="button is-warning" @click="resetRotation(1)">
                                <span class="icon">
                                    <i class="fas fa-redo"></i>
                                </span>
                            </button>
                        </b-tooltip>
                    </b-field>
                </div>
                <div class="field">
                    <label class="label">Rotacija (os Z)</label>
                    <b-field>
                        <b-field>
                            <b-tooltip label="Stupnjevi" type="is-black">
                                <b-input
                                    v-model="rotation.z.d"
                                    type="number"
                                    @input="setRotation(2)"
                                ></b-input>
                            </b-tooltip>
                            <p class="control">
                                <span class="button is-static">°</span>
                            </p>
                        </b-field>
                        <b-field>
                            <b-tooltip label="Minute" type="is-black">
                                <b-input
                                    v-model="rotation.z.m"
                                    name="minutes"
                                    type="number"
                                    @input="setRotation(2)"
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
                                    v-model="rotation.z.s"
                                    name="seconds"
                                    type="number"
                                    @input="setRotation(2)"
                                    v-validate="'numeric|min_value:0|max_value:59'"
                                ></b-input>
                            </b-tooltip>
                            <p class="control">
                                <span class="button is-static">"</span>
                            </p>
                        </b-field>
                        <b-tooltip label="Resetiraj" type="is-black">
                            <button class="button is-warning" @click="resetRotation(2)">
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
            isScale: false,
            hasCollisions: false,
            myAxis: false,
            axis: 0,
            mesh: null,
            group: 0,
            enter: {
                x: true,
                y: true,
                z: true
            },
            size: {
                x: 1,
                y: 1,
                z: 1
            },
            rotation: {
                x: {
                    d: 0,
                    m: 0,
                    s: 0
                },
                y: {
                    d: 0,
                    m: 0,
                    s: 0
                },
                z: {
                    d: 0,
                    m: 0,
                    s: 0
                }
            },
            physicsImpostor: null
        };
    },
    mounted() {
        var obj = this.getMeshByName(this.mesh);
        this.size.x = obj.scaling.x;
        this.size.y = obj.scaling.y;
        this.size.z = obj.scaling.z;
        setInterval(() => {
            this.setDMS(0);
            this.setDMS(1);
            this.setDMS(2);
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
            this.$emit("unlockPlaying");
            this.switchDrag();
        }
        if (this.isScale) {
            this.switchScale(false);
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
            existingMeshes: state => state.experiment.meshes,
            meshAxis: state => state.experiment.meshAxis,
            isPlaying: state => state.experiment.playing,
            physicsImpostors: state => state.experiment.physicsImpostors
        }),
        hasAxis: {
            get: function() {
                if (this.meshAxis.length === 0) {
                    return false;
                }
                var currentMeshAxis = this.meshAxis.find(
                    x => x.mesh === this.mesh
                );
                if (currentMeshAxis) {
                    return true;
                }
                return false;
            },
            set: function(value) {
                this.myAxis = value;
            }
        },
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
        deleteMesh() {
            var obj = this.getMeshByName(this.mesh);
            babylon.deleteMesh(obj, this.mesh);
            this.$store.commit("experiment/SET_DELETED_MESH", this.mesh);
        },
        setCurrentRotation(axis, rotation) {
            var mesh = this.getMeshByName(this.mesh);
            var quaternion = new BABYLON.Quaternion.RotationYawPitchRoll(
                rotation.y,
                rotation.x,
                rotation.z
            );
            mesh.rotationQuaternion = quaternion;
            this.setDMS(axis);
        },
        getCurrentRotation(axisString) {
            var mesh = this.getMeshByName(this.mesh);
            if (mesh == undefined) return undefined;
            return this.radianToDegree(
                mesh.rotationQuaternion.toEulerAngles()[axisString]
            );
        },
        setDMS(axis) {
            var axisString = axis === 0 ? "x" : axis === 1 ? "y" : "z";
            var rotation = this.getCurrentRotation(axisString);
            if (rotation == undefined) return;
            var deg = rotation.toFixed(2);
            if (Math.abs(deg).toFixed(2) == 0) deg = Math.abs(deg);
            this.rotation[axisString].d = Math.floor(deg);
            var minFloat = (deg - this.rotation[axisString].d) * 60;
            this.rotation[axisString].m = Math.floor(minFloat);
            var secFloat = (minFloat - this.rotation[axisString].m) * 60;
            this.rotation[axisString].s = Math.floor(secFloat);
            if (this.rotation[axisString].s == 60) {
                this.rotation[axisString].s = 0;
                this.rotation[axisString].m++;
            }
            if (this.rotation[axisString].m == 60) {
                this.rotation[axisString].m = 0;
                this.rotation[axisString].d++;
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
            var msg = `Pomicanje tijela (${this.mesh}) je omogućeno.`;
            if (!value) {
                babylon.removeDragBehaviours(
                    this.getMeshByName(this.mesh),
                    true
                );
                this.$emit("unlockPlaying");
                msg = `Pomicanje tijela (${this.mesh}). je onemogućeno.`;
            } else {
                this.$emit("lockPlaying");
                babylon.addDragBehaviour(
                    this.getMeshByName(this.mesh),
                    this.axis,
                    true
                );
            }
            this.notification(msg);
        },
        setRotation(axis) {
            var x = this.degreeToRadian(
                Number(this.rotation.x.d) +
                    Number(this.rotation.x.m) / 60 +
                    Number(this.rotation.x.s) / 3600
            );
            var y = this.degreeToRadian(
                Number(this.rotation.y.d) +
                    Number(this.rotation.y.m) / 60 +
                    Number(this.rotation.y.s) / 3600
            );
            var z = this.degreeToRadian(
                Number(this.rotation.z.d) +
                    Number(this.rotation.z.m) / 60 +
                    Number(this.rotation.z.s) / 3600
            );
            var rotation = { x: x, y: y, z: z };
            this.setCurrentRotation(axis, rotation);
        },
        resetRotation(axis) {
            var axisString = axis === 0 ? "x" : axis === 1 ? "y" : "z";
            this.rotation[axisString].d = 0;
            this.rotation[axisString].m = 0;
            this.rotation[axisString].s = 0;
            this.setRotation(axis);
        },
        switchAxis(value) {
            if (value) {
                babylon.showMeshAxis(this.mesh);
            } else {
                babylon.deleteMeshAxis(this.mesh);
            }
        },
        switchGroup(value) {
            if (value == 1) {
                if (this.isScale) {
                    this.switchScale(false);
                    this.isScale = false;
                }
            }
            this.group = value;
        },
        switchScale(value) {
            if (value) {
                var obj = this.getMeshByName(this.mesh);
                this.physicsImpostor = {
                    type: obj.physicsImpostor.type,
                    mass: obj.physicsImpostor.mass,
                    friction: obj.physicsImpostor.friction,
                    restitution: obj.physicsImpostor.restitution
                };
            } else {
                var obj = this.getMeshByName(this.mesh);
                obj.physicsImpostor = new BABYLON.PhysicsImpostor(
                    obj,
                    this.physicsImpostor.type,
                    {
                        mass: this.physicsImpostor.mass,
                        friction: this.physicsImpostor.friction,
                        restitution: this.physicsImpostor.restitution
                    },
                    this.scene
                );
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
        "size.x"(newSize, previousSize) {
            if (this.enter.x) {
                this.enter.x = false;
                return;
            }
            if (previousSize == newSize) return;
            if (this.isPlaying) {
                var obj = this.getMeshByName(this.mesh);
                obj.scaling = new BABYLON.Vector3(
                    this.size.x,
                    this.size.y,
                    this.size.z
                );
            }
        },
        "size.y"(newSize, previousSize) {
            if (this.enter.y) {
                this.enter.y = false;
                return;
            }
            if (previousSize == newSize) return;
            if (this.isPlaying) {
                var obj = this.getMeshByName(this.mesh);
                obj.scaling = new BABYLON.Vector3(
                    this.size.x,
                    this.size.y,
                    this.size.z
                );
            }
        },
        "size.z"(newSize, previousSize) {
            if (this.enter.z) {
                this.enter.z = false;
                return;
            }
            if (previousSize == newSize) return;
            if (this.isPlaying) {
                var obj = this.getMeshByName(this.mesh);
                obj.scaling = new BABYLON.Vector3(
                    this.size.x,
                    this.size.y,
                    this.size.z
                );
            }
        },
        axis: function() {
            if (this.isDrag) {
                babylon.addDragBehaviour(
                    this.getMeshByName(this.mesh),
                    this.axis
                );
            }
        },
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
                var obj = this.getMeshByName(newMesh);
                this.enter.x = true;
                this.enter.y = true;
                this.enter.z = true;
                this.size.x = obj.scaling.x;
                this.size.y = obj.scaling.y;
                this.size.z = obj.scaling.z;
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
