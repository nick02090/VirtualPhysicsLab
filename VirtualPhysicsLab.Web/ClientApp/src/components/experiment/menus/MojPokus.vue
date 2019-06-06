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

        <b-button type="is-success" icon-left="save" @click="save">Spremi pokus</b-button>

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
            objectUrl: null
        };
    },
    computed: {
        ...mapState({
            isLoading: state => state.experiment.loading,
            user: state => state.user.user
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
            createExperiment: "experiment/createExperiment"
        }),
        async validate() {
            let validationOK = await this.$validator.validateAll();
            return this.errors.any() == false && validationOK;
        },
        async save() {
            let isValid = await this.validate();
            if (!isValid) return;

            let createdBy = this.user;

            var info = {
                title: this.title,
                description: this.description,
                createdBy: createdBy
            };
            await this.createExperiment(info);

            this.notification("Uspješno ste kreirali novi pokus!");
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
