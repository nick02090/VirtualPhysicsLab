<template>
    <div id="app">
        <Navbar/>
        <router-view></router-view>
    </div>
</template>

<script>
import Navbar from "@/components/shared/Navbar.vue";

export default {
    name: "App",
    components: {
        Navbar
    },
    async mounted() {
        // await this.$store.dispatch("user/postUser");
    }
};
</script>

<style>
#app {
    font-family: "Avenir", Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #284e7b;
}
.is-primary {
    color: #284e7b !important;
}
.is-success {
    background-color: #284e7b !important;
}
.box {
    border-radius: 0px !important;
}
.icon-btn:hover {
    cursor: pointer;
    border: 0 !important;
}
.icon-btn {
    border: 0 !important;
}
a.is-active {
    background-color: #284e7b !important;
}
.tile {
    margin: 0.01px !important;
}
.margin-right {
    margin-right: auto;
}
.margin-left {
    margin-left: auto;
}
.input:focus {
    border-color: #284e7b !important;
    box-shadow: 0 0 0 0.125em #284e7b7b !important;
}
.top-padding {
    padding-top: 5px;
}
.is-turquoise {
    background-color: hsl(171, 100%, 41%);
}
.margin-5px {
    margin: 5px;
}
</style>
