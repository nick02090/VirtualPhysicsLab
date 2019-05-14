<template>
    <div id="experiment">
        <div class="tile is-ancestor">
            <div class="tile is-parent is-3 has-text-left">
                <article class="tile is-child box">
                    <Menu @notification="notification" @error="error"/>
                </article>
            </div>
            <div class="tile is-parent">
                <article class="tile is-child box">
                    <div class="content">
                        <canvas id="renderCanvas" touch-action="none"></canvas>
                    </div>
                </article>
            </div>
        </div>
    </div>
</template>

<script>
import colors from "@/helpers/colors.js";
import babylon from "@/helpers/babylon/babylon.js";
import Menu from "@/components/experiment/Menu.vue";
import { mapActions } from "vuex";

export default {
    name: "Experiment",
    data() {
        return {};
    },
    components: {
        Menu
    },
    mounted() {
        this.initScene();
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
        ...mapActions({
            createWalls: "experiment/createWalls"
        }),
        initScene() {
            var canvas = document.getElementById("renderCanvas");
            var engine = new BABYLON.Engine(canvas, true);

            var scene = babylon.createEmptyScene(canvas, engine);
            var color = colors.hexToColor3("#f5f1f2");
            scene.clearColor = new BABYLON.Color3(color[0], color[1], color[2]);

            engine.runRenderLoop(function() {
                scene.render();
            });

            window.addEventListener("resize", function() {
                engine.resize();
            });

            this.scene = scene;
            this.createWalls();
        },
        error(msg) {
            this.$toast.open({
                duration: 2500,
                message: msg,
                position: "is-bottom",
                type: "is-warning"
            });
        },
        notification(msg) {
            this.$toast.open({
                duration: 2500,
                message: msg,
                position: "is-bottom",
                type: "is-success"
            });
        }
    }
};
</script>

<style scoped>
#renderCanvas {
    width: 100%;
}
</style>
