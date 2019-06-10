<template>
    <div id="profile" v-if="currentUser">
        <div class="tile is-ancestor">
            <div class="tile is-parent is-vertical is-3">
                <article class="tile is-child notification is-success">
                    <figure class="image is-128x128">
                        <img
                            v-if="currentUser.occupation == 0"
                            class="is-rounded"
                            src="@/assets/professor.png"
                        >
                        <img v-else class="is-rounded" src="@/assets/student.png">
                    </figure>
                    <p class="title is-4" style="text-align: right;">{{currentUser.fullName}}</p>
                </article>
                <article class="tile is-child notification" style="text-align: left;">
                    <p>
                        <b>Stručnost:</b>
                        <span class="user-info">{{currentUser.occupation | occupation}}</span>
                        <br>
                        <b>E-mail:</b>
                        <span class="user-info">{{currentUser.email}}</span>
                        <br>
                        <b>Br. pokusa:</b>
                        <span class="user-info">{{experiments.length}}</span>
                        <br>
                    </p>
                </article>
            </div>
            <div class="tile is-parent is-vertical">
                <article class="tile is-child notification">
                    <b-loading :is-full-page="false" :active="experimentsLoading"></b-loading>
                    <div class="content">
                        <div class="control is-flex">
                            <p class="title is-4" style="text-align: left">Moji pokusi</p>
                            <div class="control margin-left">
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
                        </div>

                        <div class="flex-container" style="display: inline-block">
                            <div
                                class="box"
                                style="width: 30rem;"
                                v-for="experiment in filteredExperiments"
                                :key="experiment.id"
                            >
                                <article class="media">
                                    <div class="media-left">
                                        <figure class="image is-64x64">
                                            <img
                                                v-if="experiment.picture == 0"
                                                src="@/assets/black.png"
                                            >
                                            <img
                                                v-if="experiment.picture == 1"
                                                src="@/assets/blue.png"
                                            >
                                            <img
                                                v-if="experiment.picture == 2"
                                                src="@/assets/green.png"
                                            >
                                            <img
                                                v-if="experiment.picture == 3"
                                                src="@/assets/orange.png"
                                            >
                                            <img
                                                v-if="experiment.picture == 4"
                                                src="@/assets/pink.png"
                                            >
                                            <img
                                                v-if="experiment.picture == 5"
                                                src="@/assets/red.png"
                                            >
                                            <img
                                                v-if="experiment.picture == 6"
                                                src="@/assets/yellow.png"
                                            >
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
                                        <nav class="level is-mobile">
                                            <div class="level-left">
                                                <span class="margin-5px">
                                                    <b-tooltip type="is-black" label="Pokreni">
                                                        <span
                                                            class="icon is-small"
                                                            style="color: #284e7b; cursor: pointer"
                                                            @click="openExperiment(experiment.id)"
                                                        >
                                                            <i class="fas fa-external-link-alt"></i>
                                                        </span>
                                                    </b-tooltip>
                                                </span>
                                                <span class="margin-5px">
                                                    <b-tooltip type="is-black" label="Poveznica">
                                                        <span
                                                            class="icon is-small"
                                                            style="color: #284e7b; cursor: pointer"
                                                            @click="copyToClipboard(experiment.id)"
                                                        >
                                                            <i class="fas fa-copy"></i>
                                                        </span>
                                                    </b-tooltip>
                                                </span>
                                                <span
                                                    class="margin-5px"
                                                    v-if="user && currentUser.id == user.id"
                                                >
                                                    <b-tooltip type="is-black" label="Izbriši">
                                                        <span
                                                            class="icon is-small"
                                                            style="color: #ff3860; cursor: pointer"
                                                            @click="deleteExperiment(experiment.id, experiment.name)"
                                                        >
                                                            <i class="fas fa-trash-alt"></i>
                                                        </span>
                                                    </b-tooltip>
                                                </span>
                                            </div>
                                        </nav>
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
                                        :class="{'disabled': currentPage == 0 || experimentsLoading}"
                                        @click="previous"
                                    >
                                        <span class="icon">
                                            <i class="fas fa-angle-left"></i>
                                        </span>Prethodna
                                    </a>
                                    <a
                                        class="pagination-next my-pagination-button"
                                        :class="{'disabled': currentPage == pages - 1 || pages == 0 || experimentsLoading}"
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
                    </div>
                </article>
            </div>
        </div>
    </div>
</template>

<script>
import { mapState, mapGetters, mapActions } from "vuex";
import Occupation from "@/helpers/mixins/Occupation.js";

export default {
    name: "Profile",
    data() {
        return {
            currentPage: 0,
            pages: 0,
            batchSize: 3,
            searchQuery: "",
            currentUser: null
        };
    },
    mixins: [Occupation],
    async mounted() {
        const urlParams = new URLSearchParams(window.location.search);
        const id = urlParams.get("id");
        if (id) {
            await this.getUser(id);
            this.currentUser = this.profile;
            await this.getByUser(id);
        } else {
            this.currentUser = this.user;
            await this.getByUser(this.user.id);
        }
        this.pages = Math.ceil(this.experiments.length / this.batchSize);
    },
    computed: {
        ...mapState({
            user: state => state.user.user,
            experiments: state => state.experiment.experiments,
            experimentsLoading: state => state.experiment.loading,
            profile: state => state.user.profile
        }),
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
            var end = (this.currentPage + 1) * this.batchSize;
            var start = end - this.batchSize;
            return experiments.slice(start, end);
        }
    },
    methods: {
        ...mapActions({
            getByUser: "experiment/getByUser",
            deleteExperimentAsync: "experiment/deleteExperiment",
            getUser: "user/getUser"
        }),
        deleteExperiment(id, name) {
            this.$dialog.confirm({
                message: `Jeste li sigurni da želite izbrisati pokus ${name}?`,
                onConfirm: async () => {
                    await this.deleteExperimentAsync(id);
                    this.$toast.open({
                        duration: 2500,
                        message: "Pokus uspješno izbrisan.",
                        position: "is-bottom",
                        type: "is-success"
                    });
                    await this.getByUser(this.currentUser.id);
                    this.pages = Math.ceil(
                        this.experiments.length / this.batchSize
                    );
                },
                cancelText: "Odustani",
                confirmText: "Izbriši",
                type: "is-success"
            });
        },
        openExperiment(id) {
            this.$router.push(`/experiment?id=${id}`);
        },
        copyToClipboard(id) {
            let url = `${window.location.protocol}//${
                window.location.hostname
            }:${window.location.port}/experiment?id=${id}`;
            this.copy(url);
            this.$toast.open({
                duration: 2500,
                message: "Poveznica spremljena u međuspremnik.",
                position: "is-bottom",
                type: "is-success"
            });
        },
        copy(text) {
            var dummy = document.createElement("textarea");
            document.body.appendChild(dummy);
            dummy.value = text;
            dummy.select();
            document.execCommand("copy");
            document.body.removeChild(dummy);
        },
        previous() {
            if (this.currentPage > 0) {
                this.currentPage--;
            }
        },
        next() {
            if (this.currentPage < this.pages - 1) {
                this.currentPage++;
            }
        }
    }
};
</script>

<style scoped>
.user-info {
    color: black;
    margin-left: 5px;
}
.content figure {
    margin-left: 0;
    margin-right: 0;
}
.my-pagination-button {
    text-decoration: none !important;
}
.my-pagination-button.disabled {
    cursor: not-allowed;
    color: #bfbfbf !important;
}
</style>
