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
                    <article class="tile is-child box is-white">
                        <h2 class="subtitle is-9">
                            Kućica označava
                            <span class="blue-text">POČETNU</span> stranicu.
                        </h2>
                        <br>
                        <span class="icon">
                            <i class="fas fa-home fa-5x"></i>
                        </span>
                    </article>
                    <article class="tile is-child box is-white">
                        <h2 class="subtitle is-9">
                            Na vlastitom
                            <span class="blue-text">PROFILU</span> spremajte i dijelite pokuse.
                        </h2>
                        <br>
                        <span class="icon">
                            <i class="fas fa-user fa-5x"></i>
                        </span>
                    </article>
                    <article class="tile is-child box is-white">
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
        <section class="hero is-dark is-small is-bold">
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
                            Već si izradio profil?
                            <router-link to="/login">
                                <a>Prijavi se</a>
                            </router-link>
                        </p>
                    </div>
                </div>
            </div>
        </section>
    </div>
</template>

<script>
import babylon from "@/helpers/babylon/babylon.js";
import colors from "@/helpers/colors.js";

export default {
    name: "Home"
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
</style>
