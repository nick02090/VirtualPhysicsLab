<template>
    <div id="experiment">
        <div class="tile is-ancestor">
            <div class="tile is-parent is-3 has-text-left">
                <article class="tile is-child box">
                    <Menu
                        @notification="notification"
                        @error="error"
                        @unlockPlaying="unlockPlaying"
                        @lockPlaying="lockPlaying"
                    />
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
        <div class="tile is-ancestor">
            <div class="tile is-parent is-3">
                <article class="tile is-child box">
                    <b-tooltip
                        type="is-black"
                        label="Zaustavi fiziku."
                        position="is-right"
                        v-if="isPlaying"
                    >
                        <b-button
                            class="is-success"
                            size="is-small"
                            icon-left="pause"
                            :disabled="isDisabled"
                            @click="togglePlay(false)"
                        >Fizika uključena</b-button>
                    </b-tooltip>
                    <b-tooltip
                        type="is-black"
                        label="Pokreni fiziku."
                        position="is-right"
                        v-if="!isPlaying"
                    >
                        <b-button
                            class="is-dark"
                            size="is-small"
                            icon-left="play"
                            :disabled="isDisabled"
                            @click="togglePlay(true)"
                        >Fizika isključena</b-button>
                    </b-tooltip>
                </article>
            </div>
        </div>

        <b-modal :active.sync="firstOpen" @close="close" width="700px">
            <header class="modal-card-head is-flex" style="text-align: left">
                <p class="modal-card-title">Odabir pokusa</p>
                <div class="control margin-left" v-if="group == 1">
                    <b-field>
                        <b-input
                            placeholder="Pretraži..."
                            type="search"
                            icon-pack="fas"
                            icon="search"
                        ></b-input>
                    </b-field>
                </div>
            </header>
            <section class="modal-card-body">
                <nav class="breadcrumb is-centered has-bullet-separator" aria-label="breadcrumbs">
                    <ul>
                        <li :class="{'is-active': group == 0}">
                            <a @click="group = 0">
                                <span class="icon is-small">
                                    <i class="fas fa-rocket" aria-hidden="true"></i>
                                </span>
                                <span>Novi pokus</span>
                            </a>
                        </li>
                        <li :class="{'is-active': group == 1}" v-if="isLoggedIn">
                            <a @click="group = 1">
                                <span class="icon is-small">
                                    <i class="fas fa-book" aria-hidden="true"></i>
                                </span>
                                <span>Moji pokusi</span>
                            </a>
                        </li>
                    </ul>
                </nav>
                <hr>
                <div v-if="group == 0">
                    <div
                        class="box margin-right"
                        style="width: 20rem;"
                        :class="{'selected': selected0 == 0}"
                        @click="selected = 0"
                    >
                        <article class="media">
                            <div class="media-left">
                                <figure class="image is-64x64">
                                    <img src="@/assets/emptyScene.png">
                                </figure>
                            </div>
                            <div class="media-content">
                                <div class="content">
                                    <p>
                                        <strong>Prazan pokus</strong>
                                        <br>
                                        <small>Prazna scena postavljena s podlogom.</small>
                                    </p>
                                </div>
                            </div>
                        </article>
                    </div>
                </div>
                <div v-if="group == 1">
                    <div
                        class="box margin-right margin-left"
                        style="width: 20rem;"
                        v-for="(experiment, index) in filteredExperiments"
                        :key="experiment.id"
                        :class="{'selected': selected1 == index}"
                        @click="selected1 = index"
                    >
                        <article class="media">
                            <div class="media-left">
                                <figure class="image is-64x64">
                                    <img :src="experiment.image">
                                </figure>
                            </div>
                            <div class="media-content">
                                <div class="content">
                                    <p>
                                        <strong>{{experiment.title}}</strong>
                                        <br>
                                        <small>{{experiment.description}}</small>
                                    </p>
                                </div>
                            </div>
                        </article>
                    </div>
                    <div>
                        <nav
                            class="pagination is-rounded"
                            role="navigation"
                            aria-label="pagination"
                        >
                            <a
                                class="pagination-previous my-pagination-button"
                                :class="{'disabled': currentPage == 0}"
                                @click="previous"
                            >
                                <span class="icon">
                                    <i class="fas fa-angle-left"></i>
                                </span>Prethodna
                            </a>
                            <a
                                class="pagination-next my-pagination-button"
                                :class="{'disabled': currentPage == pages - 1}"
                                @click="next"
                            >
                                Sljedeća
                                <span class="icon">
                                    <i class="fas fa-angle-right"></i>
                                </span>
                            </a>
                        </nav>
                    </div>
                </div>
            </section>
            <footer class="modal-card-foot is-flex">
                <b-button
                    class="button is-success margin-left"
                    icon-left="check"
                    @click="close"
                >Pokreni</b-button>
            </footer>
        </b-modal>
    </div>
</template>

<script>
import colors from "@/helpers/colors.js";
import babylon from "@/helpers/babylon/babylon.js";
import Menu from "@/components/experiment/Menu.vue";
import { mapActions, mapState, mapGetters } from "vuex";

export default {
    name: "Experiment",
    data() {
        return {
            isDisabled: false,
            firstOpen: true,
            group: 0,
            selected0: 0,
            selected1: 0,
            currentPage: 0,
            pages: 0,
            batchSize: 3,
            filteredExperiments: []
        };
    },
    components: {
        Menu
    },
    async mounted() {
        if (this.isLoggedIn) {
            await this.getByUser(this.user.id);
            this.pages = Math.ceil(this.experiments.length / this.batchSize);
            this.update();
        }
    },
    computed: {
        ...mapState({
            isPlaying: state => state.experiment.playing,
            user: state => state.user.user,
            experiments: state => state.experiment.experiments
        }),
        ...mapGetters({
            isLoggedIn: "user/isLoggedIn"
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
    methods: {
        ...mapActions({
            createWalls: "experiment/createWalls",
            getByUser: "experiment/getByUser"
        }),
        previous() {
            if (this.currentPage > 0) {
                this.selected1 = 0;
                this.currentPage--;
                this.update();
            }
        },
        next() {
            if (this.currentPage < this.pages - 1) {
                this.selected1 = 0;
                this.currentPage++;
                this.update();
            }
        },
        update() {
            var end = (this.currentPage + 1) * this.batchSize;
            var start = end - this.batchSize;
            this.filteredExperiments = this.experiments.slice(start, end);
        },
        close() {
            this.initScene();
            if (this.group == 1) {
                this.loadScene();
            }
            this.firstOpen = false;
        },
        unlockPlaying() {
            this.isDisabled = false;
        },
        lockPlaying() {
            this.isDisabled = true;
        },
        togglePlay(data) {
            this.$store.commit("experiment/SET_PLAYING", data);
            babylon.togglePlay(data);
        },
        loadScene() {
            console.log("učitavanje...");
        },
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
.box {
    cursor: pointer;
}
.box.selected {
    background-color: #f9f9f9;
    box-shadow: 1px 1px 10px black;
    z-index: 20;
}
.my-pagination-button {
    text-decoration: none !important;
}
.my-pagination-button.disabled {
    cursor: not-allowed;
    color: #bfbfbf !important;
}
</style>
