<template>
    <div id="novi-element">
        <div class="first-stage" v-if="stage === 0">
            <div class="field">
                <label class="label">Postavke</label>
                <b-field :type="errors.has('mesh') ? 'is-danger' : ''">
                    <b-input
                        placeholder="Naziv"
                        v-model="properties.name"
                        icon="signature"
                        name="mesh"
                        maxlength="4"
                        v-validate="'required|max:4'"
                    ></b-input>
                </b-field>
                <b-field :type="errors.has('mass') ? 'is-danger' : ''">
                    <b-input
                        placeholder="Masa"
                        v-model="properties.mass"
                        icon="weight-hanging"
                        name="mass"
                        v-validate="'required|decimal|min_value:0'"
                        expanded
                    ></b-input>
                    <p class="control">
                        <span class="button is-static">kg</span>
                    </p>
                </b-field>
                <my-color-picker @input="updateColor"></my-color-picker>
            </div>
            <div class="field">
                <label class="label">Vrsta oblika</label>
                <div class="control is-flex">
                    <div class="select">
                        <select v-model="meshType">
                            <option
                                v-for="mesh in meshTypes"
                                :key="mesh"
                                :value="mesh"
                            >{{mesh | translateMeshType}}</option>
                        </select>
                    </div>
                    <div class="control margin-left">
                        <button class="button is-success" @click="addMesh">
                            <span class="icon">
                                <i class="fas fa-plus"></i>
                            </span>
                            <span>Dodaj</span>
                        </button>
                    </div>
                </div>
            </div>
            <div class="field" v-if="false">
                <label class="label">Preslikaj postojeći</label>
                <div class="control is-flex">
                    <div class="select">
                        <select v-model="mesh">
                            <option
                                v-for="mesh in existingMeshes"
                                :key="mesh"
                                :value="mesh"
                            >{{mesh}}</option>
                        </select>
                    </div>
                    <div class="control margin-left">
                        <button class="button is-success" disabled>
                            <span class="icon">
                                <i class="fas fa-copy"></i>
                            </span>
                            <span>Preslikaj</span>
                        </button>
                    </div>
                </div>
            </div>
        </div>
        <div class="second-stage" v-if="stage === 1">
            <div class="field">
                <label class="label">Pozicioniranje</label>
                <p>Smjestite element na sceni tako da nije u dodiru s drugima.</p>
                <div class="block">
                    <b-radio v-model="axis" :native-value="0">x</b-radio>
                    <b-radio v-model="axis" :native-value="1">y</b-radio>
                    <b-radio v-model="axis" :native-value="2">z</b-radio>
                </div>
            </div>
            <div class="control is-flex">
                <div class="control margin-right">
                    <button class="button" @click="back">
                        <span class="icon">
                            <i class="fas fa-angle-left"></i>
                        </span>
                        <span>Nazad</span>
                    </button>
                </div>
                <div class="control margin-left">
                    <b-tooltip v-if="hasCollisions" type="is-black" label="Postoje kolizije!">
                        <button class="button is-success" disabled>
                            <span class="icon">
                                <i class="fas fa-plus"></i>
                            </span>
                            <span>Dodaj</span>
                        </button>
                    </b-tooltip>
                    <button v-else class="button is-success" @click="finish">
                        <span class="icon">
                            <i class="fas fa-plus"></i>
                        </span>
                        <span>Dodaj</span>
                    </button>
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
import MyColorPicker from "@/components/shared/MyColorPicker.vue";
import { clearInterval } from "timers";

export default {
    name: "NoviElement",
    data() {
        return {
            meshType: meshTypes.BOX,
            meshTypes: meshTypes,
            mesh: null,
            stage: 0,
            axis: 0,
            hasCollisions: false,
            isSuccess: false,
            properties: {
                name: "",
                width: 2,
                height: 2,
                depth: 2,
                position: {
                    x: 0,
                    y: 0,
                    z: 0
                },
                mass: 1
            },
            color: "#284E7B"
        };
    },
    beforeDestroy() {
        if (this.stage === 1 && !this.isSuccess) {
            this.deleteMesh();
            this.error("Element nije uspješno dodan!");
        }
    },
    components: {
        MyColorPicker
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
        }
    },
    filters: {
        translateMeshType(value) {
            switch (value) {
                case "BOX":
                    return "Kocka";
                case "SPHERE":
                    return "Kugla";
                case "CYLINDER":
                    return "Valjak";
                default:
                    return;
            }
        }
    },
    methods: {
        togglePicker() {
            this.isPickerActive = !this.isPickerActive;
        },
        updateColor(value) {
            this.color = value;
        },
        deleteMesh() {
            babylon.deleteMesh(this.getMeshByName(this.mesh), this.mesh);
        },
        checkCollisions() {
            if (this.stage === 1) {
                var obj = this.getMeshByName(this.mesh);
                if (obj) {
                    this.hasCollisions = babylon.checkCollisions(
                        obj,
                        this.mesh
                    );
                }
            }
        },
        back() {
            this.deleteMesh();
            this.stage = 0;
        },
        finish() {
            babylon.toggleHighlight(this.getMeshByName(this.mesh));
            babylon.removeDragBehaviours(this.getMeshByName(this.mesh), true);
            this.isSuccess = true;
            this.notification(
                `Uspješno ste stvorili novi element: ${this.mesh}`
            );
            this.$emit("changeMenu", {
                groupName: null,
                menuName: null
            });
        },
        async validate() {
            let validationOK = await this.$validator.validateAll();
            return this.errors.any() == false && validationOK;
        },
        async addMesh() {
            let isValid = await this.validate();
            if (!isValid) return;

            if (this.getMeshByName(this.properties.name)) {
                this.errors.add({
                    field: "mesh",
                    msg: "Već postoji element s tim nazivom!"
                });
                this.error(
                    `Već postoji element s nazivom: ${this.properties.name}`
                );
                return;
            }

            var color = colors.hexToColor3(this.color);
            var material = new BABYLON.StandardMaterial("material", this.scene);
            material.diffuseColor = new BABYLON.Color3(
                color[0],
                color[1],
                color[2]
            );
            var mesh = babylon.addMesh(
                this.meshType,
                this.properties,
                material,
                this.scene
            );
            this.mesh = mesh.name;

            this.stage = 1;
            babylon.toggleHighlight(this.getMeshByName(this.mesh));
            setInterval(() => {
                this.checkCollisions();
            }, 1);
            babylon.addDragBehaviour(
                this.getMeshByName(this.mesh),
                this.axis,
                true
            );
        },
        notification(msg) {
            this.$emit("notification", msg);
        },
        error(msg) {
            this.$emit("error", msg);
        }
    },
    watch: {
        axis: function() {
            if (this.stage === 1) {
                babylon.addDragBehaviour(
                    this.getMeshByName(this.mesh),
                    this.axis
                );
            }
        }
    }
};
</script>

<style scoped>
</style>
