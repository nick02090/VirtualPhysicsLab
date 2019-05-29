<template>
    <div id="radno-okruzenje">
        <div class="control is-flex">
            <div class="control margin-right">
                <label class="label">Koordinatne osi</label>
            </div>
            <div class="control margin-left">
                <b-tooltip
                    type="is-black"
                    label="Uključi/Isključi prikaz globalnih koordinatnih osi."
                >
                    <b-switch v-model="hasAxis" @input="switchAxis"></b-switch>
                </b-tooltip>
            </div>
        </div>
        <div class="control is-flex">
            <div class="control margin-right">
                <label class="label">Zidovi okruženja</label>
            </div>
            <div class="control margin-left">
                <b-tooltip type="is-black" label="Postavi/Izbriši zidove radnog okruženja.">
                    <b-switch v-model="hasWalls" @input="switchWalls"></b-switch>
                </b-tooltip>
            </div>
        </div>
        <div class="field">
            <label class="label">Položaj kamere</label>
            <div class="buttons">
                <b-button
                    class="is-success"
                    rounded
                    icon-left="arrow-down"
                    @click="cameraView('down')"
                >z-x</b-button>
                <b-button
                    class="is-success"
                    rounded
                    icon-left="arrow-left"
                    @click="cameraView('left')"
                >y-z</b-button>
                <b-button
                    class="is-success"
                    rounded
                    icon-left="arrow-up"
                    @click="cameraView('up')"
                >y-x</b-button>
                <b-button class="is-success" rounded icon-left="home" @click="cameraView('home')"></b-button>
            </div>
        </div>
        <div class="field">
            <label class="label">Faktori trenja</label>
            <b-field>
                <b-tooltip
                    type="is-black"
                    label="Faktor trenja podloge i zidova"
                    position="is-right"
                >
                    <b-numberinput
                        :step="0.01"
                        v-model="friction"
                        min="0"
                        controls-position="compact"
                        controls-rounded
                        type="is-success"
                        :editable="false"
                    ></b-numberinput>
                </b-tooltip>
                <b-icon icon="road" class="margin-5px"></b-icon>
            </b-field>
        </div>
        <div class="field">
            <label class="label">Koeficijenti elastičnosti</label>
            <b-field>
                <b-tooltip
                    type="is-black"
                    label="Koeficijent elastičnosti podloge i zidova"
                    position="is-right"
                >
                    <b-numberinput
                        :step="0.01"
                        v-model="restitution"
                        min="0"
                        max="1"
                        controls-position="compact"
                        controls-rounded
                        type="is-success"
                        :editable="false"
                    ></b-numberinput>
                </b-tooltip>
                <b-icon icon="expand-arrows-alt" class="margin-5px"></b-icon>
            </b-field>
        </div>
    </div>
</template>

<script>
import babylon from "@/helpers/babylon/babylon.js";
import { mapGetters, mapState, mapActions } from "vuex";

export default {
    name: "RadnoOkruzenje",
    data() {
        return {
            axis: true,
            walls: true
        };
    },
    computed: {
        ...mapState({
            currentAxis: state => state.experiment.axis,
            currentWalls: state => state.experiment.walls,
            ground: state => state.experiment.ground,
            camera: state => state.experiment.camera
        }),
        hasAxis: {
            get: function() {
                if (this.currentAxis.length > 0) {
                    return true;
                }
                return false;
            },
            set: function(value) {
                this.axis = value;
            }
        },
        hasWalls: {
            get: function() {
                if (this.currentWalls.length > 0) {
                    return true;
                }
                return false;
            },
            set: function(value) {
                this.walls = value;
            }
        },
        friction: {
            get: function() {
                return this.ground.physicsImpostor.friction;
            },
            set: function(value) {
                this.ground.physicsImpostor.friction = value;
            }
        },
        restitution: {
            get: function() {
                return this.ground.physicsImpostor.restitution;
            },
            set: function(value) {
                this.ground.physicsImpostor.restitution = value;
            }
        },
        scene: {
            get: function() {
                return this.$store.state.experiment.scene;
            },
            set: function(data) {
                this.$store.commit("experiment/SET_SCENE", data);
            }
        }
    },
    methods: {
        ...mapActions({
            createWalls: "experiment/createWalls",
            deleteWalls: "experiment/deleteWalls"
        }),
        switchAxis(value) {
            if (value) {
                babylon.showAxis();
            } else {
                babylon.removeAxis();
            }
        },
        switchWalls(value) {
            if (value) {
                this.createWalls();
            } else {
                this.deleteWalls();
            }
        },
        cameraView(view) {
            switch (view) {
                case "down":
                    this.camera.setPosition(new BABYLON.Vector3(5, 30, 0));
                    break;
                case "left":
                    this.camera.setPosition(new BABYLON.Vector3(25, -0.5, 0));
                    break;
                case "up":
                    this.camera.setPosition(new BABYLON.Vector3(0, -0.5, -25));
                    break;
                case "home":
                    this.camera.setPosition(new BABYLON.Vector3(10, 6, -20));
                    break;
                default:
                    break;
            }
        }
    }
};
</script>

<style scoped>
</style>
