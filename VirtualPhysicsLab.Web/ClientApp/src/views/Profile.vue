<template>
    <div id="profile">
        <div class="tile is-ancestor">
            <div class="tile is-parent is-vertical is-3">
                <article class="tile is-child notification is-success">
                    <figure class="image is-128x128">
                        <img
                            class="is-rounded"
                            src="https://bulma.io/images/placeholders/128x128.png"
                        >
                    </figure>
                    <p class="title is-4" style="text-align: right;">{{user.fullName}}</p>
                </article>
                <article class="tile is-child notification" style="text-align: left;">
                    <p>
                        <b>Stručnost:</b>
                        <span class="user-info">{{user.occupation}}</span>
                        <br>
                        <b>E-mail:</b>
                        <span class="user-info">{{user.email}}</span>
                        <br>
                        <b>Br. pokusa:</b>
                        <span class="user-info">{{user.experiments.length}}</span>
                        <br>
                    </p>
                </article>
            </div>
            <div class="tile is-parent is-vertical">
                <article class="tile is-child notification">
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
                                            <img :src="experiment.image" alt="Image">
                                        </figure>
                                    </div>
                                    <div class="media-content">
                                        <div class="content">
                                            <p>
                                                <strong>{{experiment.title}}</strong>
                                                <small
                                                    style="margin-left: 5px"
                                                >{{experiment.date.toLocaleDateString("hr-HR")}}</small>
                                                <br>
                                                <small>{{experiment.description}}</small>
                                            </p>
                                        </div>
                                        <nav class="level is-mobile">
                                            <div class="level-left">
                                                <b-tooltip type="is-black" label="Pokreni">
                                                    <span
                                                        class="icon is-small"
                                                        style="color: #284e7b;"
                                                    >
                                                        <i class="fas fa-link" aria-hidden="true"></i>
                                                    </span>
                                                </b-tooltip>
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
                    </div>
                </article>
            </div>
        </div>
    </div>
</template>

<script>
import { mapState, mapGetters } from "vuex";

export default {
    name: "Profile",
    data() {
        return {
            currentPage: 0,
            pages: 0,
            batchSize: 3,
            filteredExperiments: [],
            user: {
                fullName: "Nikola Ćališ",
                occupation: "student",
                email: "nikola.calis9@gmail.com",
                experiments: [
                    {
                        id: 1,
                        title: "Pomak",
                        date: new Date(2019, 5, 3),
                        description:
                            "Prikaz pomaka tijela u prostoru. Još nešto da nadopunim.",
                        image:
                            "https://bulma.io/images/placeholders/256x256.png"
                    },
                    {
                        id: 2,
                        title: "Pomak",
                        date: new Date(2019, 4, 30),
                        description: "Prikaz pomaka tijela u prostoru.",
                        image:
                            "https://bulma.io/images/placeholders/256x256.png"
                    },
                    {
                        id: 3,
                        title: "Pomak",
                        date: new Date(2019, 4, 25),
                        description: "Prikaz pomaka tijela u prostoru.",
                        image:
                            "https://bulma.io/images/placeholders/256x256.png"
                    },
                    {
                        id: 4,
                        title: "Pomak",
                        date: new Date(2019, 4, 25),
                        description: "Prikaz pomaka tijela u prostoru.",
                        image:
                            "https://bulma.io/images/placeholders/256x256.png"
                    }
                ]
            }
        };
    },
    mounted() {
        this.pages = Math.ceil(this.user.experiments.length / this.batchSize);
        this.update();
    },
    computed: {
        ...mapGetters({
            isLoggedIn: "user/isLoggedIn"
        })
    },
    methods: {
        previous() {
            if (this.currentPage > 0) {
                this.currentPage--;
                this.update();
            }
        },
        next() {
            if (this.currentPage < this.pages - 1) {
                this.currentPage++;
                this.update();
            }
        },
        update() {
            var end = (this.currentPage + 1) * this.batchSize;
            var start = end - this.batchSize;
            this.filteredExperiments = this.user.experiments.slice(start, end);
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
