<template>
    <div id="rotacija">ROTACIJA</div>
</template>

<script>
export default {
    name: "Rotacija",
    props: {
        log: {
            required: true,
            type: Object
        },
        mesh: {
            required: true,
            type: String
        }
    }
};
</script>

<style scoped>
</style>
