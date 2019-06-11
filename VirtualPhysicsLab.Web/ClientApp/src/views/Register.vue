<template>
    <div id="register">
        <section class="hero is-info is-bold">
            <div class="hero-body">
                <div class="container">
                    <h1 class="title">Registracija</h1>
                </div>
            </div>
        </section>
        <section class="hero is-light is-bold">
            <div class="hero-body">
                <div class="tile is-parent">
                    <div class="tile is-child is-4">
                        <div class="control is-flex">
                            <label class="label">Ime:</label>
                            <div class="control margin-left">
                                <b-field :type="errors.has('firstName') ? 'is-danger' : ''">
                                    <b-input
                                        v-model="firstName"
                                        expanded
                                        name="firstName"
                                        v-validate="'required'"
                                        icon="user"
                                    ></b-input>
                                </b-field>
                            </div>
                        </div>

                        <div class="control is-flex">
                            <label class="label">Prezime:</label>
                            <div class="control margin-left">
                                <b-field :type="errors.has('lastName') ? 'is-danger' : ''">
                                    <b-input
                                        v-model="lastName"
                                        expanded
                                        name="lastName"
                                        v-validate="'required'"
                                        icon="signature"
                                    ></b-input>
                                </b-field>
                            </div>
                        </div>

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
                            <label class="label">Stručnost:</label>
                            <div class="control margin-left">
                                <div class="block">
                                    <b-radio v-model="occupation" :native-value="0">profesor</b-radio>
                                    <b-radio v-model="occupation" :native-value="1">student</b-radio>
                                </div>
                            </div>
                        </div>

                        <div class="control is-flex">
                            <div class="control margin-left">
                                <b-button
                                    type="is-success"
                                    @click="register"
                                    icon-left="check"
                                >Registracija</b-button>
                            </div>
                        </div>
                        <hr>
                        <p style="text-align: left">
                            <small style="color: red">* Sva su polja obavezna.</small>
                            <br>
                            <small style="color: red">* Lozinka mora sadržavati barem 8 znakova.</small>
                        </p>
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
                        Već imaš profil?
                        <router-link to="/login">
                            <a>Prijavi se</a>
                        </router-link>
                    </p>
                    <div class="control margin-left"></div>
                </div>
            </div>
        </section>
    </div>
</template>

<script>
import { mapState, mapGetters, mapActions } from "vuex";

export default {
    name: "Register",
    data() {
        return {
            firstName: "",
            lastName: "",
            email: "",
            password: "",
            occupation: 0
        };
    },
    computed: {
        ...mapState({
            isLoading: state => state.user.loading
        })
    },
    methods: {
        ...mapActions({
            checkAvailability: "user/checkAvailability",
            createUser: "user/createUser",
            authenticate: "user/authenticate"
        }),
        async validate() {
            let validationOK = await this.$validator.validateAll();
            return this.errors.any() == false && validationOK;
        },
        async register() {
            let isValid = await this.validate();
            if (!isValid) return;

            var user = {
                firstName: this.firstName,
                lastName: this.lastName,
                email: this.email,
                password: this.password,
                occupation: this.occupation
            };
            let isAvailable = await this.checkAvailability(user);
            if (isAvailable) {
                this.$toast.open({
                    message: "Provjera dostupnosti uspješna!",
                    type: "is-success",
                    position: "is-bottom"
                });
                await this.createUser(user);
                this.$toast.open({
                    message: "Uspješno ste se registrirali!",
                    type: "is-success",
                    position: "is-bottom"
                });
                this.$router.push("login");
            } else {
                this.$toast.open({
                    message: "Navedeni e-mail se već koristi!",
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
