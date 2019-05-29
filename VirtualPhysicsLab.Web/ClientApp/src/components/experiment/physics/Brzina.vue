<template>
    <div id="brzina">
        <b-field label="Orijentacija" horizontal>
            <div class="block">
                <b-radio v-model="axis" :native-value="0">x</b-radio>
                <b-radio v-model="axis" :native-value="1">y</b-radio>
                <b-radio v-model="axis" :native-value="2">z</b-radio>
            </div>
        </b-field>
        <b-field label="Vrijednost" horizontal :type="errors.has('velocity') ? 'is-danger' : ''">
            <b-input
                v-model="velocity"
                icon="angle-double-right"
                name="velocity"
                v-validate="'required|decimal'"
            ></b-input>
        </b-field>
    </div>
</template>

<script>
export default {
    name: "Brzina",
    data() {
        return {
            axis: 0,
            velocity: null
        };
    },
    methods: {
        async validate() {
            let validationOK = await this.$validator.validateAll();
            return this.errors.any() == false && validationOK;
        },
        async addPhysic() {
            let isValid = await this.validate();
            if (!isValid) return;

            var axisString =
                this.axis === 0 ? "x" : this.axis === 1 ? "y" : "z";

            var physic = {
                name: "Brzina",
                icon: "fas fa-running",
                description: `${axisString}-os`,
                properties: {
                    value: this.velocity,
                    axis: this.axis,
                    axisString: axisString,
                    unit: "m/s"
                }
            };

            this.$emit("addPhysic", physic);
        }
    }
};
</script>

<style scoped>
</style>