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
            isLoading: state => state.user.loading
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
        async validate() {
            let validationOK = await this.$validator.validateAll();
            return this.errors.any() == false && validationOK;
        },
        async save() {
            let isValid = await this.validate();
            if (!isValid) return;

            this.doDownload(this.title, this.scene);

            this.notification("Yaaay!");
        },
        doDownload(filename, scene) {
            if (this.objectUrl) {
                window.URL.revokeObjectURL(this.objectUrl);
            }

            var serializedScene = BABYLON.SceneSerializer.Serialize(scene);

            var strScene = JSON.stringify(serializedScene);

            filename += ".babylon";

            var blob = new Blob([strScene], { type: "octet/stream" });

            // turn blob into an object URL; saved as a member, so can be cleaned out later
            this.objectUrl = (window.webkitURL || window.URL).createObjectURL(
                blob
            );

            var link = window.document.createElement("a");
            link.href = this.objectUrl;
            link.download = filename;
            var click = document.createEvent("MouseEvents");
            click.initEvent("click", true, false);
            link.dispatchEvent(click);
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
