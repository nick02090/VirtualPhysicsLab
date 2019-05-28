<template>
    <div id="fizika">FIZIKA</div>
</template>

<script>
export default {
    name: "Fizika",
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
