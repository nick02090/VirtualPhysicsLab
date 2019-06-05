<template>
    <div id="navbar">
        <section class="hero is-light is-flex">
            <div class="brand">
                <div class="control is-flex">
                    <div v-if="isLoggedIn" class="control is-flex" style="display: inline-block">
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
                { isLink: true, url: "/home" },
                { isLink: true, url: "/experiment" },
                { isLink: true, url: "/profile" }
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
        async logout() {
            await this.systemLogout();
            this.$toast.open({
                message: "Uspješno ste se odjavili!",
                type: "is-success",
                position: "is-bottom"
            });
            this.$router.push("home");
        }
    },
    watch: {
        $route(to, from) {
            if (this.$refs.quickmenu.$el.classList.contains("active")) {
                this.$refs.quickmenu.$el.classList.remove("active");
            }
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
