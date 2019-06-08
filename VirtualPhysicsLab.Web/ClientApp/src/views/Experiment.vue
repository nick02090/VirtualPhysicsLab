<template>
    <div id="experiment">
        <div class="tile is-ancestor">
            <b-loading :is-full-page="false" :active="sceneLoading"></b-loading>
            <div class="tile is-parent is-3 has-text-left">
                <article class="tile is-child box">
                    <Menu
                        @notification="notification"
                        @error="error"
                        @unlockPlaying="unlockPlaying"
                        @lockPlaying="lockPlaying"
                        ref="menu"
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
            <b-loading :is-full-page="false" :active="sceneLoading"></b-loading>
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
                            :disabled="isDisabled || sceneLoading"
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
                            :disabled="isDisabled || sceneLoading"
                            @click="togglePlay(true)"
                        >Fizika isključena</b-button>
                    </b-tooltip>
                </article>
            </div>
        </div>

        <b-modal :active.sync="firstOpen" @close="close" width="700px" scroll="keep">
            <header class="modal-card-head is-flex" style="text-align: left">
                <p class="modal-card-title">Odabir pokusa</p>
                <div class="control margin-left" v-if="group == 1">
                    <b-field>
                        <b-input
                            placeholder="Pretraži..."
                            type="search"
                            icon-pack="fas"
                            icon="search"
                            v-model="searchQuery"
                        ></b-input>
                    </b-field>
                </div>
            </header>
            <section class="modal-card-body">
                <b-loading :is-full-page="false" :active="experimentsLoading"></b-loading>
                <nav class="breadcrumb is-centered has-bullet-separator" aria-label="breadcrumbs">
                    <ul>
                        <li :class="{'is-active': group == 0}">
                            <a @click="group = 0; lockCreate = false">
                                <span class="icon is-small">
                                    <i class="fas fa-rocket" aria-hidden="true"></i>
                                </span>
                                <span>Novi pokus</span>
                            </a>
                        </li>
                        <li
                            :class="{'is-active': group == 1}"
                            v-if="isLoggedIn && experiments.length > 0"
                        >
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
                                        <small
                                            style="margin-left: 5px"
                                        >{{new Date(experiment.createdOn).toLocaleDateString("hr-HR")}}</small>
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
                    :disabled="lockCreate"
                >Pokreni</b-button>
            </footer>
        </b-modal>
    </div>
</template>

<script>
import colors from "@/helpers/colors.js";
import babylon from "@/helpers/babylon/babylon.js";
import Menu from "@/components/experiment/Menu.vue";
import MojPokus from "@/components/experiment/menus/MojPokus.vue";
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
            engine: null,
            searchQuery: "",
            lockCreate: false,
            sceneLoading: false
        };
    },
    components: {
        Menu
    },
    beforeDestroy() {
        let allMeshes = Array.from(this.existingMeshes);
        for (var i in allMeshes) {
            var mesh = allMeshes[i];
            var obj = this.getMeshByName(mesh);
            babylon.deleteMesh(obj, mesh);
        }
    },
    async mounted() {
        const urlParams = new URLSearchParams(window.location.search);
        const id = urlParams.get("id");
        if (id) {
            this.firstOpen = false;
            this.initScene();
            this.sceneLoading = true;
            this.engine.displayLoadingUI();
            this.engine.loadingUIText = "Dohvaćanje pokusa...";
            await this.getExperiment(id);
            if (this.isLoggedIn) {
                if (this.experiment.createdBy.id == this.user.id) {
                    this.$refs["menu"].menus.push({
                        name: "Postavke",
                        menus: [
                            {
                                name: "Moj pokus",
                                component: MojPokus
                            }
                        ]
                    });
                }
            }
            await this.loadScene(this.experiment);
            return;
        }
        if (this.isLoggedIn) {
            this.$refs["menu"].menus.push({
                name: "Postavke",
                menus: [
                    {
                        name: "Moj pokus",
                        component: MojPokus
                    }
                ]
            });
            await this.getByUser(this.user.id);
            this.pages = Math.ceil(this.experiments.length / this.batchSize);
        }
    },
    computed: {
        ...mapState({
            isPlaying: state => state.experiment.playing,
            user: state => state.user.user,
            experiments: state => state.experiment.experiments,
            meshes: state => state.experiment.experimentMeshes,
            experimentsLoading: state => state.experiment.loading,
            experiment: state => state.experiment.experiment,
            existingMeshes: state => state.experiment.meshes
        }),
        ...mapGetters({
            isLoggedIn: "user/isLoggedIn",
            getMeshByName: "experiment/getMeshByName"
        }),
        scene: {
            get: function() {
                return this.$store.state.experiment.scene;
            },
            set: function(data) {
                this.$store.commit("experiment/SET_SCENE", data);
            }
        },
        filteredExperiments() {
            var experiments = [];
            if (this.searchQuery && this.searchQuery.length == 0) {
                experiments = this.experiments;
            } else {
                var regex = new RegExp(this.searchQuery, "i");

                let preparedEntities = this.experiments.map(e => ({
                    ...e,
                    filterObject: {
                        title: e.title,
                        description: e.description,
                        createdOn: new Date(e.createdOn).toLocaleString("hr-HR")
                    }
                }));
                experiments = preparedEntities.filter(e =>
                    Object.keys(e.filterObject).some(key =>
                        regex.test(e.filterObject[key])
                    )
                );
            }
            this.selected1 = 0;
            var end = (this.currentPage + 1) * this.batchSize;
            var start = end - this.batchSize;
            var result = experiments.slice(start, end);
            if (result.length == 0 && this.group == 1) {
                this.lockCreate = true;
            } else {
                this.lockCreate = false;
            }
            return result;
        }
    },
    methods: {
        ...mapActions({
            createWalls: "experiment/createWalls",
            getByUser: "experiment/getByUser",
            getMeshes: "experiment/getMeshes",
            getExperiment: "experiment/getExperiment"
        }),
        previous() {
            if (this.currentPage > 0) {
                this.selected1 = 0;
                this.currentPage--;
            }
        },
        next() {
            if (this.currentPage < this.pages - 1) {
                this.selected1 = 0;
                this.currentPage++;
            }
        },
        close() {
            this.initScene();
            if (this.group == 1) {
                var experiment = this.filteredExperiments[this.selected1];
                var newurl =
                    window.location.protocol +
                    "//" +
                    window.location.host +
                    window.location.pathname +
                    `?id=${experiment.id}`;
                window.history.pushState({ path: newurl }, "", newurl);
                this.loadScene(experiment);
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
        async loadScene(experiment) {
            this.sceneLoading = true;
            this.engine.displayLoadingUI();
            this.engine.loadingUIText = "Dohvaćanje podataka...";
            await this.getMeshes(experiment.id);
            this.engine.loadingUIText = "Postavljanje elemenata na scenu...";
            babylon.loadScene(this.scene, this.meshes, experiment);
            this.engine.hideLoadingUI();
            this.sceneLoading = false;
        },
        initScene() {
            var canvas = document.getElementById("renderCanvas");
            this.engine = new BABYLON.Engine(canvas, true);

            var scene = babylon.createEmptyScene(canvas, this.engine);
            var color = colors.hexToColor3("#f5f1f2");
            scene.clearColor = new BABYLON.Color3(color[0], color[1], color[2]);

            this.engine.runRenderLoop(function() {
                scene.render();
            });

            window.addEventListener("resize", function() {
                this.engine.resize();
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
