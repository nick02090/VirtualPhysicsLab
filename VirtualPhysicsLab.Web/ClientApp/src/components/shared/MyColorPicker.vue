<template>
    <div id="my-color-picker">
        <div class="is-flex top-padding">
            <b-input placeholder="Boja" :value="color" disabled></b-input>
            <button class="button disabled" :style="{'background-color': color}"></button>
            <div class="control margin-left">
                <button class="button is-warning" @click="isPickerActive = true">
                    <span class="icon">
                        <i class="fas fa-palette"></i>
                    </span>
                </button>
            </div>
        </div>
        <b-modal :active.sync="isPickerActive" scroll="keep" width="225px">
            <color-picker class="color-picker" :value="color" @input="updateColor"></color-picker>
        </b-modal>
    </div>
</template>

<script>
import { Chrome as ColorPicker } from "vue-color";

export default {
    name: "MyColorPicker",
    data() {
        return {
            color: "#284E7B",
            isPickerActive: false
        };
    },
    components: {
        ColorPicker
    },
    methods: {
        updateColor(value) {
            this.color = value.hex;
            this.$emit("input", this.color);
        }
    }
};
</script>

<style scoped>
button.button.disabled {
    cursor: context-menu;
    border: none;
    box-shadow: none;
}
</style>
