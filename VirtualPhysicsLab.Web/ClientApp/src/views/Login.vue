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
                                <b-field :type="errors.has('email') ? 'is-danger' : ''">
                                    <b-input
                                        type="email"
                                        v-model="email"
                                        expanded
                                        name="email"
                                        icon="envelope"
                                        v-validate="'required|email'"
                                    ></b-input>
                                </b-field>
                            </div>
                        </div>

                        <div class="control is-flex">
                            <label class="label">Lozinka:</label>
                            <div class="control margin-left">
                                <b-field :type="errors.has('password') ? 'is-danger' : ''">
                                    <b-input
                                        type="password"
                                        v-model="password"
                                        password-reveal
                                        expanded
                                        name="password"
                                        v-validate="'required|min:8'"
                                    ></b-input>
                                </b-field>
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
            <b-loading :is-full-page="false" :active="isLoading"></b-loading>
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
import { mapActions, mapState } from "vuex";

export default {
    name: "Login",
    data() {
        return {
            email: "",
            password: ""
        };
    },
    computed: {
        ...mapState({
            isLoading: state => state.user.loading
        })
    },
    methods: {
        ...mapActions({
            authenticate: "user/authenticate"
        }),
        async validate() {
            let validationOK = await this.$validator.validateAll();
            return this.errors.any() == false && validationOK;
        },
        async login() {
            let isValid = await this.validate();
            if (!isValid) return;

            var user = {
                email: this.email,
                password: this.password
            };
            let success = await this.authenticate(user);
            if (success === true) {
                this.$toast.open({
                    message: "Uspješno ste se prijavili!",
                    type: "is-success",
                    position: "is-bottom"
                });
                this.$router.push("home");
            } else {
                this.$toast.open({
                    message: "Neispravan e-mail ili lozinka!",
                    type: "is-danger",
                    position: "is-bottom"
                });
            }
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
