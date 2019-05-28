<template>
    <div id="pomak">
        <div class="block">
            <b-radio v-model="graphType" :native-value="0">Životna linija</b-radio>
            <!-- <b-radio v-model="graphType" :native-value="1">Brzina</b-radio>
            <b-radio v-model="graphType" :native-value="2">Sudari</b-radio>
            <b-radio v-model="graphType" :native-value="3">Rotacija</b-radio>-->
        </div>
        <p v-if="graphType === 0">
            <b>Životna linija</b>
            predstavlja promjenu položaja tijela {{this.mesh}} od trenutka njegovog stvaranja (0 s - stvoren element {{this.mesh}}).
        </p>
        <p v-if="graphType === 1"></p>
        <p v-if="graphType === 2"></p>
        <p v-if="graphType === 3"></p>
        <basic-movement-chart v-if="graphType === 0 && showGraph" :graphData="graphData"/>
        <velocity-movement-chart v-if="graphType === 1 && showGraph" :graphData="graphData"/>
        <collision-movement-chart v-if="graphType === 2 && showGraph" :graphData="graphData"/>
        <rotation-movement-chart v-if="graphType === 3 && showGraph" :graphData="graphData"/>
    </div>
</template>

<script>
import BasicMovementChart from "@/components/experiment/charts/movement/BasicMovementChart.vue";
import VelocityMovementChart from "@/components/experiment/charts/movement/VelocityMovementChart.vue";
import CollisionMovementChart from "@/components/experiment/charts/movement/CollisionMovementChart.vue";
import RotationMovementChart from "@/components/experiment/charts/movement/RotationMovementChart.vue";
import { mapGetters, mapState } from "vuex";

export default {
    name: "Pomak",
    props: {
        log: {
            required: true,
            type: Object
        },
        mesh: {
            required: true,
            type: String
        }
    },
    data() {
        return {
            graphType: 0,
            lifeTime: 0,
            graphData: null,
            showGraph: false
        };
    },
    components: {
        BasicMovementChart,
        VelocityMovementChart,
        CollisionMovementChart,
        RotationMovementChart
    },
    mounted() {
        this.graphType = 0;
        var allLogs = this.getMeshLog(this.mesh);
        var lifeLog = allLogs.find(x => x.type === "life");
        this.lifeTime = lifeLog.properties.time;
        this.findCross(this.graphType);
        this.showGraph = true;
    },
    computed: {
        ...mapGetters({
            getMeshLog: "experiment/getMeshLog"
        })
    },
    methods: {
        findCross(graphType) {
            var allLogs = this.getMeshLog(this.mesh);
            switch (graphType) {
                case 0:
                    this.emptyCross(allLogs);
                    break;
                case 1:
                    this.velocityCross(allLogs);
                    break;
                case 2:
                    var collisionLogs = allLogs.filter(
                        x => x.type === "collisions"
                    );
                    break;
                case 3:
                    var rotationLogs = allLogs.filter(
                        x => x.type === "rotation"
                    );
                    break;
                case 4:
                    var movementLogs = allLogs.filter(
                        x => x.type === "movement" && x.key !== this.log.key
                    );
                    break;
                default:
                    break;
            }
        },
        emptyCross(allLogs) {
            var positions = this.log.properties.positions;

            var series = [];
            var categories = [];

            this.graphData = {
                series: series,
                categories: categories
            };
        },
        velocityCross(allLogs) {}
    }
};
</script>

<style scoped>
</style>
