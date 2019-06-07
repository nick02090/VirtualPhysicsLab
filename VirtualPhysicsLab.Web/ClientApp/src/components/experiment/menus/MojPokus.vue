<template>
    <div id="moj-pokus">
        <b-field label="Naziv" :type="errors.has('title') ? 'is-danger' : ''">
            <b-input v-model="title" name="title" v-validate="'required'"></b-input>
        </b-field>

        <b-field label="Opis" :type="errors.has('description') ? 'is-danger' : ''">
            <b-input
                type="textarea"
                v-model="description"
                maxlength="120"
                name="description"
                v-validate="'required|max:120'"
            ></b-input>
        </b-field>

        <div class="buttons">
            <b-button type="is-success" icon-left="save" @click="save">Novi pokus</b-button>
            <b-button type="is-warning" icon-left="pen" @click="update" v-if="id">Ažuriraj pokus</b-button>
        </div>

        <b-loading :is-full-page="true" :active="isLoading"></b-loading>
    </div>
</template>

<script>
import babylon from "@/helpers/babylon/babylon.js";
import { mapGetters, mapState, mapActions } from "vuex";

export default {
    name: "MojPokus",
    data() {
        return {
            title: "",
            description: "",
            id: null
        };
    },
    mounted() {
        const urlParams = new URLSearchParams(window.location.search);
        this.id = urlParams.get("id");
        if (this.id) {
            this.title = this.experiment.title;
            this.description = this.experiment.description;
        }
    },
    computed: {
        ...mapState({
            isLoading: state => state.experiment.loading,
            user: state => state.user.user,
            experiment: state => state.experiment.experiment
        }),
        scene: {
            get: function() {
                return this.$store.state.experiment.scene;
            },
            set: function(data) {
                this.$store.commit("experiment/SET_SCENE", data);
            }
        }
    },
    methods: {
        ...mapActions({
            createExperiment: "experiment/createExperiment",
            checkAvailability: "experiment/checkAvailability",
            updateExperiment: "experiment/updateExperiment"
        }),
        async validate() {
            let validationOK = await this.$validator.validateAll();
            return this.errors.any() == false && validationOK;
        },
        async save() {
            let isValid = await this.validate();
            if (!isValid) return;

            let createdBy = this.user;

            var newExperiment = {
                title: this.title,
                createdBy: createdBy
            };
            let isAvailable = await this.checkAvailability(newExperiment);
            if (!isAvailable) {
                this.error("Postoji već pokus s tim nazivom!");
                return;
            }

            var info = {
                title: this.title,
                description: this.description,
                createdBy: createdBy
            };
            await this.createExperiment(info);

            var experiment = this.experiment;
            var newurl =
                window.location.protocol +
                "//" +
                window.location.host +
                window.location.pathname +
                `?id=${experiment.id}`;
            window.history.pushState({ path: newurl }, "", newurl);

            this.notification("Uspješno ste kreirali novi pokus!");
            this.changeMenu(null, null);
        },
        async update() {
            let isValid = await this.validate();
            if (!isValid) return;

            let createdBy = this.user;
            var info = {
                title: this.title,
                description: this.description,
                createdBy: createdBy,
                id: this.id
            };
            await this.updateExperiment(info);

            this.notification(`Uspješno ste ažurirali pokus (${this.title})!`);
            this.changeMenu(null, null);
        },
        notification(msg) {
            this.$emit("notification", msg);
        },
        error(msg) {
            this.$emit("error", msg);
        },
        changeMenu(groupName, menuName) {
            this.$emit("changeMenu", {
                groupName: groupName,
                menuName: menuName
            });
        }
    }
};
</script>

<style scoped>
</style>
