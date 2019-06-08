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
                                v-model="searchQuery"
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
                                    <img :src="user.image">
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
                                                    @click="copyToClipboard(user.id)"
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
                                :class="{'disabled': currentPage == 0}"
                                @click="previous"
                            >
                                <span class="icon">
                                    <i class="fas fa-angle-left"></i>
                                </span>Prethodna
                            </a>
                            <a
                                class="pagination-next my-pagination-button"
                                :class="{'disabled': currentPage == pages - 1 || pages == 0}"
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
            currentPage: 0,
            pages: 0,
            batchSize: 5,
            searchQuery: ""
        };
    },
    mixins: [Occupation],
    async mounted() {
        await this.getUsersAsync();
        this.users = this.getUsers;
        this.pages = Math.ceil(this.users.length / this.batchSize);
    },
    computed: {
        ...mapGetters({
            isLoggedIn: "user/isLoggedIn",
            getUsers: "user/getUsers"
        }),
        ...mapState({
            user: state => state.user.user
        }),
        filteredUsers() {
            var users = [];
            if (this.searchQuery && this.searchQuery.length == 0) {
                users = this.users;
            } else {
                var regex = new RegExp(this.searchQuery, "i");

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
            var end = (this.currentPage + 1) * this.batchSize;
            var start = end - this.batchSize;
            return users.slice(start, end);
        }
    },
    methods: {
        ...mapActions({
            getUsersAsync: "user/getUsersAsync"
        }),
        changeRoute(route) {
            this.$router.push(route);
        },
        openUser(id) {
            this.$router.push(`/profile?id=${id}`);
        },
        copyToClipboard(id) {
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
