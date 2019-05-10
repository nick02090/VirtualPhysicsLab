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
    </div>
</template>

<script>
import babylon from "@/helpers/babylon/babylon.js";
import { mapGetters, mapState } from "vuex";

export default {
    name: "RadnoOkruzenje",
    data() {
        return {
            hasAxis: true
        };
    },
    computed: {
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
        switchAxis(value) {
            if (value) {
                babylon.showAxis();
            } else {
                babylon.removeAxis();
            }
        }
    }
};
</script>

<style scoped>
</style>
