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
            <label class="label">Postavke</label>
            <b-tooltip type="is-black" label="Faktor trenja podloge" position="is-right">
                <b-field :type="errors.has('friction') ? 'is-danger' : ''">
                    <b-input
                        placeholder="Faktor trenja"
                        v-model="friction"
                        icon="weight-hanging"
                        name="friction"
                        v-validate="'required|decimal|min_value:0'"
                        expanded
                    ></b-input>
                </b-field>
            </b-tooltip>
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
            ground: state => state.experiment.ground
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
        }
    }
};
</script>

<style scoped>
</style>
