<template>
    <div id="login">
        <section class="hero is-info is-bold">
            <div class="hero-body">
                <div class="container">
                    <h1 class="title">Prijava</h1>
                </div>
            </div>
        </section>
        <section class="hero is-light is-bold">
            <div class="hero-body">
                <div class="tile is-parent">
                    <div class="tile is-child is-4">
                        <div class="control is-flex">
                            <label class="label">E-mail:</label>
                            <div class="control margin-left">
                                <b-input
                                    type="email"
                                    v-model="email"
                                    expanded
                                    name="email"
                                    icon="envelope"
                                    v-validate="'required|email'"
                                ></b-input>
                            </div>
                        </div>

                        <div class="control is-flex">
                            <label class="label">Lozinka:</label>
                            <div class="control margin-left">
                                <b-input
                                    type="password"
                                    v-model="password"
                                    password-reveal
                                    expanded
                                    name="password"
                                    v-validate="'required|min:8'"
                                ></b-input>
                            </div>
                        </div>

                        <div class="control is-flex">
                            <div class="control margin-left">
                                <b-button type="is-success" @click="login" icon-left="check">Prijava</b-button>
                            </div>
                        </div>
                    </div>
                    <div class="tile is-child has-text-centered">
                        <router-link to="/">
                            <img src="@/assets/logo.png" width="225" height="28">
                        </router-link>
                    </div>
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
                    <div class="control margin-left"></div>
                </div>
            </div>
        </section>
    </div>
</template>

<script>
export default {
    name: "Login",
    data() {
        return {
            email: "",
            password: ""
        };
    },
    methods: {
        async validate() {
            let validationOK = await this.$validator.validateAll();
            return this.errors.any() == false && validationOK;
        },
        async login() {
            let isValid = await this.validate();
            if (!isValid) return;

            this.$toast.open({
                message: "Yaaaay!",
                type: "is-success"
            });
        }
    }
};
</script>

<style scoped>
a {
    color: #6bafff;
}
a:hover {
    color: white;
}
</style>
