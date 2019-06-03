<template>
    <div id="navbar">
        <section class="hero is-light is-flex">
            <div class="brand">
                <router-link to="/">
                    <img src="@/assets/logo.png" width="112" height="28">
                </router-link>
            </div>
            <quick-menu
                :menu-count="3"
                :icon-class="icons"
                :menu-url-list="urls"
                position="top-right"
                background-color="#284e7b"
                ref="quickmenu"
            ></quick-menu>
        </section>
    </div>
</template>

<script>
import quickMenu from "vue-quick-menu";

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
    components: {
        quickMenu
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
