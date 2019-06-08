<template>
    <div id="navbar">
        <section class="hero is-light is-flex">
            <div class="brand">
                <div class="control is-flex">
                    <div v-if="isLoggedIn" class="control is-flex">
                        <b-button rounded outlined icon-left="sign-out-alt" @click="logout">Odjava</b-button>
                    </div>
                    <div class="control margin-right margin-left">
                        <router-link to="/">
                            <img src="@/assets/logo.png" width="112" height="28">
                        </router-link>
                    </div>
                </div>
            </div>
            <quick-menu
                :menu-count="3"
                :icon-class="icons"
                :menu-url-list="urls"
                @process="process"
                position="top-right"
                background-color="#284e7b"
                ref="quickmenu"
            ></quick-menu>
            <b-loading :is-full-page="true" :active="isLoading"></b-loading>
        </section>
    </div>
</template>

<script>
import quickMenu from "vue-quick-menu";
import { mapGetters, mapActions, mapState } from "vuex";

export default {
    name: "Navbar",
    data() {
        return {
            icons: ["fas fa-home", "fas fa-flask", "fas fa-user"],
            urls: [
                { isLink: false, url: "/home" },
                { isLink: false, url: "/experiment" },
                { isLink: false, url: "/profile" }
            ]
        };
    },
    computed: {
        ...mapGetters({
            isLoggedIn: "user/isLoggedIn"
        }),
        ...mapState({
            isLoading: state => state.user.loading
        })
    },
    components: {
        quickMenu
    },
    methods: {
        ...mapActions({
            systemLogout: "user/logout"
        }),
        process(index) {
            switch (index) {
                case 0:
                    this.change("home");
                    break;
                case 1:
                    this.change("experiment");
                    break;
                case 2:
                    this.change("profile");
                    break;
                default:
                    break;
            }
        },
        change(to) {
            var path = window.location.pathname;
            if (to == "experiment") {
                if (path.includes("experiment")) {
                    var newurl =
                        window.location.protocol +
                        "//" +
                        window.location.host +
                        "/experiment";
                    window.location.replace(newurl);
                    this.$refs.quickmenu.$el.classList.remove("active");
                    return;
                } else {
                    this.$router.push("experiment");
                    this.$refs.quickmenu.$el.classList.remove("active");
                    return;
                }
            }
            if (to == "profile") {
                if (path.includes("profile")) {
                    var newurl =
                        window.location.protocol +
                        "//" +
                        window.location.host +
                        "/profile";
                    window.location.replace(newurl);
                    this.$refs.quickmenu.$el.classList.remove("active");
                    return;
                } else {
                    this.$router.push("profile");
                    this.$refs.quickmenu.$el.classList.remove("active");
                    return;
                }
            }
            this.$router.push(to);
            this.$refs.quickmenu.$el.classList.remove("active");
        },
        async logout() {
            await this.systemLogout();
            this.$toast.open({
                message: "Uspješno ste se odjavili!",
                type: "is-success",
                position: "is-bottom"
            });
            this.$router.push("home");
        }
    }
};
</script>

<style scoped>
section.hero {
    height: 50px;
}
.brand {
    margin: 10px;
}
.quick-menu {
    top: 15px !important;
    right: 15px !important;
    cursor: pointer;
    z-index: 100;
}
</style>
