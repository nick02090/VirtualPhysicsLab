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
                    <b-switch v-model="hasWalls" @input="switchWalls" :disabled="isScale"></b-switch>
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
        <div class="control is-flex">
            <div class="control margin-right">
                <label class="label">Gravitacijska sila</label>
            </div>
            <div class="control margin-left" style="text-align: right">
                <div class="block">
                    <b-radio v-model="gravityAxis" :native-value="0" :disabled="isPlaying">x</b-radio>
                    <b-radio v-model="gravityAxis" :native-value="1" :disabled="isPlaying">y</b-radio>
                    <b-radio v-model="gravityAxis" :native-value="2" :disabled="isPlaying">z</b-radio>
                </div>
            </div>
        </div>
        <b-field>
            <b-numberinput
                :step="0.01"
                v-model="gravity"
                controls-position="compact"
                controls-rounded
                type="is-success"
                :disabled="isPlaying"
            ></b-numberinput>
            <b-icon icon="apple-alt" class="margin-5px"></b-icon>
        </b-field>
        <div class="control is-flex">
            <div class="control margin-right">
                <label class="label">Veličina</label>
            </div>
            <div class="control margin-left">
                <b-tooltip type="is-black" label="Uključi/Isključi podešavanje veličine podloge.">
                    <b-switch v-model="isScale" @input="switchScale" :disabled="!isPlaying"></b-switch>
                </b-tooltip>
            </div>
        </div>
        <b-field horizontal label="x-os">
            <b-numberinput
                :step="0.1"
                v-model="size.x"
                :min="1"
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
                :min="1"
                :max="5"
                controls-position="compact"
                controls-rounded
                type="is-success"
                :editable="false"
                expanded
                :disabled="!isScale"
            ></b-numberinput>
        </b-field>
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
            walls: true,
            gravity: -9.81,
            gravityAxis: 1,
            size: {
                x: 1,
                y: 1,
                z: 1
            },
            enter: {
                x: true,
                y: true,
                z: true
            },
            isScale: false,
            physicsImpostor: null
        };
    },
    mounted() {
        this.gravityAxis = this.storeGravityAxis;
        if (this.gravityAxis == 0) {
            this.gravity = this.storeGravity.x;
        } else if (this.gravityAxis == 1) {
            this.gravity = this.storeGravity.y;
        } else {
            this.gravity = this.storeGravity.z;
        }
        var ground = this.ground;
        this.size.x = ground.scaling.x;
        this.size.y = ground.scaling.y;
        this.size.z = ground.scaling.z;
    },
    beforeDestroy() {
        if (this.isScale) {
            this.switchScale(false);
        }
    },
    computed: {
        ...mapState({
            currentAxis: state => state.experiment.axis,
            currentWalls: state => state.experiment.walls,
            ground: state => state.experiment.ground,
            camera: state => state.experiment.camera,
            isPlaying: state => state.experiment.playing,
            storeGravity: state => state.experiment.gravity,
            storeGravityAxis: state => state.experiment.gravityAxis
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
                this.deleteWalls();
                this.createWalls();
            }
        },
        restitution: {
            get: function() {
                return this.ground.physicsImpostor.restitution;
            },
            set: function(value) {
                this.ground.physicsImpostor.restitution = value;
                this.deleteWalls();
                this.createWalls();
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
        changeGravity() {
            var gravityVector = new BABYLON.Vector3(
                this.gravityAxis == 0 ? this.gravity : 0,
                this.gravityAxis == 1 ? this.gravity : 0,
                this.gravityAxis == 2 ? this.gravity : 0
            );
            this.$store.commit("experiment/SET_GRAVITY", gravityVector);
            this.$store.commit("experiment/SET_GRAVITY_AXIS", this.gravityAxis);
        },
        switchScale(value) {
            if (value) {
                var ground = this.ground;
                this.physicsImpostor = {
                    type: ground.physicsImpostor.type,
                    mass: ground.physicsImpostor.mass,
                    friction: ground.physicsImpostor.friction,
                    restitution: ground.physicsImpostor.restitution
                };
                this.deleteWalls();
            } else {
                var ground = this.ground;
                ground.physicsImpostor = new BABYLON.PhysicsImpostor(
                    ground,
                    this.physicsImpostor.type,
                    {
                        mass: this.physicsImpostor.mass,
                        friction: this.physicsImpostor.friction,
                        restitution: this.physicsImpostor.restitution
                    },
                    this.scene
                );
                this.createWalls();
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
    },
    watch: {
        gravity() {
            this.changeGravity();
        },
        gravityAxis() {
            this.changeGravity();
        },
        "size.x"(newSize, previousSize) {
            if (this.enter.x) {
                this.enter.x = false;
                return;
            }
            if (previousSize == newSize) return;
            if (this.isPlaying) {
                var ground = this.ground;
                ground.scaling = new BABYLON.Vector3(
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
                var ground = this.ground;
                ground.scaling = new BABYLON.Vector3(
                    this.size.x,
                    this.size.y,
                    this.size.z
                );
            }
        }
    }
};
</script>

<style scoped>
</style>
