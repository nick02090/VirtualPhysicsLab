<template>
    <div id="home">
        <section class="hero is-info is-medium is-bold">
            <div class="hero-body">
                <div class="container">
                    <h2 class="subtitle">Dobrodošli u ...</h2>
                    <h1 class="title">Virtualni laboratoriji iz fizike</h1>
                </div>
            </div>
        </section>
        <section class="hero is-light is-small is-bold">
            <div class="hero-body">
                <div class="tile is-parent">
                    <article class="tile is-child box is-white">
                        <h2 class="subtitle is-9">
                            Navigirajte se stranicom koristeći
                            <span class="blue-text">IZBORNIK</span>.
                        </h2>
                        <br>
                        <span class="icon">
                            <i class="fas fa-bars fa-5x"></i>
                        </span>
                    </article>
                    <article class="tile is-child box is-white" @click="changeRoute('home')">
                        <h2 class="subtitle is-9">
                            Kućica označava
                            <span class="blue-text">POČETNU</span> stranicu.
                        </h2>
                        <br>
                        <span class="icon">
                            <i class="fas fa-home fa-5x"></i>
                        </span>
                    </article>
                    <article class="tile is-child box is-white" @click="changeRoute('profile')">
                        <h2 class="subtitle is-9">
                            Na vlastitom
                            <span class="blue-text">PROFILU</span> spremajte i dijelite pokuse.
                        </h2>
                        <br>
                        <span class="icon">
                            <i class="fas fa-user fa-5x"></i>
                        </span>
                    </article>
                    <article class="tile is-child box is-white" @click="changeRoute('experiment')">
                        <h2 class="subtitle is-9">
                            Izradite vlastite
                            <span class="blue-text">POKUSE</span> iz fizike u 3D okruženju.
                        </h2>
                        <br>
                        <span class="icon">
                            <i class="fas fa-flask fa-5x"></i>
                        </span>
                    </article>
                </div>
            </div>
        </section>
        <section class="hero is-dark is-small is-bold" v-if="!isLoggedIn">
            <div class="hero-body">
                <div class="control is-flex">
                    <p>
                        Još nemaš izrađen profil?
                        <router-link to="/register">
                            <a>Registriraj se</a>
                        </router-link>
                    </p>
                    <div class="control margin-left">
                        <p>
                            Već imaš profil?
                            <router-link to="/login">
                                <a>Prijavi se</a>
                            </router-link>
                        </p>
                    </div>
                </div>
            </div>
        </section>
        <section class="hero is-dark is-small is-bold" v-else>
            <div class="hero-body">
                <div class="control is-flex">
                    <p>
                        Prijavljeni ste kao
                        <a @click="changeRoute('profile')">{{user.fullName}}</a>
                    </p>
                    <div class="control margin-left"></div>
                </div>
            </div>
        </section>
        <section class="hero is-light is-bold">
            <div class="hero-body">
                <div class="control is-flex">
                    <h1 class="title is-4">Korisnici</h1>
                    <div class="control margin-left">
                        <b-field>
                            <b-input
                                placeholder="Pretraži..."
                                type="search"
                                icon-pack="fas"
                                icon="search"
                                v-model="userSearch.searchQuery"
                            ></b-input>
                        </b-field>
                    </div>
                </div>

                <div class="flex-container" style="display: inline-block">
                    <div
                        class="box"
                        style="width: 30rem;"
                        v-for="user in filteredUsers"
                        :key="user.id"
                    >
                        <article class="media">
                            <div class="media-left">
                                <figure class="image is-64x64">
                                    <img v-if="user.occupation == 0" src="@/assets/professor.png">
                                    <img v-else src="@/assets/student.png">
                                </figure>
                            </div>
                            <div class="media-content">
                                <div class="content">
                                    <p>
                                        <strong>{{user.fullName}}</strong>
                                        <small
                                            style="margin-left: 5px"
                                        >({{user.occupation | occupation}})</small>
                                        <br>
                                        <small>{{user.email}}</small>
                                    </p>
                                </div>
                                <nav class="level is-mobile">
                                    <div class="level-left">
                                        <span class="margin-5px">
                                            <b-tooltip type="is-black" label="Profil">
                                                <span
                                                    class="icon is-small"
                                                    style="color: #284e7b; cursor: pointer"
                                                    @click="openUser(user.id)"
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
                                                    @click="copyUserToClipboard(user.id)"
                                                >
                                                    <i class="fas fa-copy"></i>
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
                                :class="{'disabled': userSearch.currentPage == 0}"
                                @click="previousUser"
                            >
                                <span class="icon">
                                    <i class="fas fa-angle-left"></i>
                                </span>Prethodna
                            </a>
                            <a
                                class="pagination-next my-pagination-button"
                                :class="{'disabled': userSearch.currentPage == userSearch.pages - 1 || userSearch.pages == 0}"
                                @click="nextUser"
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
        </section>
        <hr>
        <section class="hero is-light is-bold">
            <div class="hero-body">
                <div class="control is-flex">
                    <h1 class="title is-4">Pokusi</h1>
                    <div class="control margin-left">
                        <b-field>
                            <b-input
                                placeholder="Pretraži..."
                                type="search"
                                icon-pack="fas"
                                icon="search"
                                v-model="experimentSearch.searchQuery"
                            ></b-input>
                        </b-field>
                    </div>
                </div>

                <div class="flex-container" style="display: inline-block">
                    <b-loading :is-full-page="false" :active="experimentsLoading"></b-loading>
                    <div
                        class="box"
                        style="width: 30rem;"
                        v-for="experiment in filteredExperiments"
                        :key="experiment.id"
                    >
                        <article class="media">
                            <div class="media-left">
                                <figure class="image is-64x64">
                                    <img v-if="experiment.picture == 0" src="@/assets/black.png">
                                    <img v-if="experiment.picture == 1" src="@/assets/blue.png">
                                    <img v-if="experiment.picture == 2" src="@/assets/green.png">
                                    <img v-if="experiment.picture == 3" src="@/assets/orange.png">
                                    <img v-if="experiment.picture == 4" src="@/assets/pink.png">
                                    <img v-if="experiment.picture == 5" src="@/assets/red.png">
                                    <img v-if="experiment.picture == 6" src="@/assets/yellow.png">
                                </figure>
                            </div>
                            <div class="media-content">
                                <div class="content">
                                    <p>
                                        <strong>{{experiment.title}}</strong>
                                        <small
                                            style="margin-left: 5px"
                                        >{{experiment.createdBy.fullName}}</small>
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
                                                    @click="copyExperimentToClipboard(experiment.id)"
                                                >
                                                    <i class="fas fa-copy"></i>
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
                                :class="{'disabled': experimentSearch.currentPage == 0}"
                                @click="previousExperiment"
                            >
                                <span class="icon">
                                    <i class="fas fa-angle-left"></i>
                                </span>Prethodna
                            </a>
                            <a
                                class="pagination-next my-pagination-button"
                                :class="{'disabled': experimentSearch.currentPage == experimentSearch.pages - 1 || experimentSearch.pages == 0}"
                                @click="nextExperiment"
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
        </section>
    </div>
</template>

<script>
import { mapGetters, mapState, mapActions } from "vuex";
import Occupation from "@/helpers/mixins/Occupation.js";

export default {
    name: "Home",
    data() {
        return {
            users: [],
            experiments: [],
            userSearch: {
                currentPage: 0,
                pages: 0,
                batchSize: 3,
                searchQuery: ""
            },
            experimentSearch: {
                currentPage: 0,
                pages: 0,
                batchSize: 3,
                searchQuery: ""
            }
        };
    },
    mixins: [Occupation],
    async mounted() {
        await this.getExperimentsAsync();
        await this.getUsersAsync();
        this.users = this.getUsers;
        this.userSearch.pages = Math.ceil(
            this.users.length / this.userSearch.batchSize
        );
        this.experiments = this.getExperiments;
        this.experimentSearch.pages = Math.ceil(
            this.experiments.length / this.experimentSearch.batchSize
        );
    },
    computed: {
        ...mapGetters({
            isLoggedIn: "user/isLoggedIn",
            getUsers: "user/getUsers",
            getExperiments: "experiment/getExperiments"
        }),
        ...mapState({
            user: state => state.user.user,
            experimentsLoading: state => state.experiment.loading
        }),
        filteredUsers() {
            var users = [];
            if (
                this.userSearch.searchQuery &&
                this.userSearch.searchQuery.length == 0
            ) {
                users = this.users;
            } else {
                var regex = new RegExp(this.userSearch.searchQuery, "i");

                let preparedEntities = this.users.map(e => ({
                    ...e,
                    filterObject: {
                        fullName: e.fullName,
                        occupation: this.$options.filters.occupation(
                            e.occupation
                        ),
                        email: e.email
                    }
                }));
                users = preparedEntities.filter(e =>
                    Object.keys(e.filterObject).some(key =>
                        regex.test(e.filterObject[key])
                    )
                );
            }
            var end =
                (this.userSearch.currentPage + 1) * this.userSearch.batchSize;
            var start = end - this.userSearch.batchSize;
            return users.slice(start, end);
        },
        filteredExperiments() {
            var experiments = [];
            if (
                this.experimentSearch.searchQuery &&
                this.experimentSearch.searchQuery.length == 0
            ) {
                experiments = this.experiments;
            } else {
                var regex = new RegExp(this.experimentSearch.searchQuery, "i");

                let preparedEntities = this.experiments.map(e => ({
                    ...e,
                    filterObject: {
                        title: e.title,
                        description: e.description,
                        fullName: e.createdBy.fullName
                    }
                }));
                experiments = preparedEntities.filter(e =>
                    Object.keys(e.filterObject).some(key =>
                        regex.test(e.filterObject[key])
                    )
                );
            }
            var end =
                (this.experimentSearch.currentPage + 1) *
                this.experimentSearch.batchSize;
            var start = end - this.experimentSearch.batchSize;
            return experiments.slice(start, end);
        }
    },
    methods: {
        ...mapActions({
            getUsersAsync: "user/getUsersAsync",
            getExperimentsAsync: "experiment/getExperimentsAsync"
        }),
        changeRoute(route) {
            this.$router.push(route);
        },
        openUser(id) {
            this.$router.push(`/profile?id=${id}`);
        },
        openExperiment(id) {
            this.$router.push(`/experiment?id=${id}`);
        },
        copyUserToClipboard(id) {
            let url = `${window.location.protocol}//${
                window.location.hostname
            }:${window.location.port}/profile?id=${id}`;
            this.copy(url);
            this.$toast.open({
                duration: 2500,
                message: "Poveznica spremljena u međuspremnik.",
                position: "is-bottom",
                type: "is-success"
            });
        },
        copyExperimentToClipboard(id) {
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
        previousUser() {
            if (this.userSearch.currentPage > 0) {
                this.userSearch.currentPage--;
            }
        },
        nextUser() {
            if (this.userSearch.currentPage < this.userSearch.pages - 1) {
                this.userSearch.currentPage++;
            }
        },
        previousExperiment() {
            if (this.experimentSearch.currentPage > 0) {
                this.experimentSearch.currentPage--;
            }
        },
        nextExperiment() {
            if (
                this.experimentSearch.currentPage <
                this.experimentSearch.pages - 1
            ) {
                this.experimentSearch.currentPage++;
            }
        }
    }
};
</script>

<style scoped>
i.fas {
    color: #284e7b;
}
span.blue-text {
    color: #284e7b;
}
a {
    color: #6bafff;
}
a:hover {
    color: white;
}
.box:hover {
    background-color: #e2e2e2;
    box-shadow: 1px 1px 20px black;
    z-index: 20;
    cursor: pointer;
}
.my-pagination-button {
    text-decoration: none !important;
}
.my-pagination-button.disabled {
    cursor: not-allowed;
    color: #bfbfbf !important;
}
</style>
