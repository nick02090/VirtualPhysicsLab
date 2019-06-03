<template>
    <div id="sudari">
        <div class="block">
            <b-radio v-model="graphType" :native-value="0">Životna linija</b-radio>
            <!-- <b-radio v-model="graphType" :native-value="1">Brzina</b-radio>
            <b-radio v-model="graphType" :native-value="2">Pomak</b-radio>
            <b-radio v-model="graphType" :native-value="3">Rotacija</b-radio>-->
        </div>
        <p v-if="graphType === 0">
            <b>Životna linija</b>
            predstavlja trenutak sudara tijela {{this.mesh}} s tijelom {{this.log.properties.collider}} od trenutka stvaranja promatranog tijela (0 s - stvoren element {{this.mesh}}).
        </p>
        <p v-if="graphType === 1">
            <b>Brzina</b>
            predstavlja brzinu tijela {{this.mesh}} netom prije i nakon sudara s tijelom {{this.log.properties.collider}} (0 s - stvoren element {{this.mesh}}).
        </p>
        <p v-if="graphType === 2"></p>
        <p v-if="graphType === 3"></p>
        <basic-collision-chart v-if="graphType === 0 && showGraph" :graphData="graphData"/>
        <velocity-collision-chart v-if="graphType === 1 && showGraph" :graphData="graphData"/>
        <movement-collision-chart v-if="graphType === 2 && showGraph" :graphData="graphData"/>
        <rotation-collision-chart v-if="graphType === 3 && showGraph" :graphData="graphData"/>
    </div>
</template>

<script>
import BasicCollisionChart from "@/components/experiment/charts/collisions/BasicCollisionChart.vue";
import VelocityCollisionChart from "@/components/experiment/charts/collisions/VelocityCollisionChart.vue";
import MovementCollisionChart from "@/components/experiment/charts/collisions/MovementCollisionChart.vue";
import RotationCollisionChart from "@/components/experiment/charts/collisions/RotationCollisionChart.vue";
import { mapGetters, mapState } from "vuex";

export default {
    name: "Sudari",
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
            excel: {
                data: []
            },
            graphData: null,
            showGraph: false
        };
    },
    components: {
        BasicCollisionChart,
        VelocityCollisionChart,
        MovementCollisionChart,
        RotationCollisionChart
    },
    mounted() {
        this.graphType = 0;
        var allLogs = this.getMeshLog(this.mesh);
        var lifeLog = allLogs.find(x => x.type === "life");
        this.lifeTime = lifeLog.properties.time;
        this.findCross(this.graphType);
        this.showGraph = true;
        this.prepareExcel();
        this.$emit("excel");
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
                    var movementLogs = allLogs.filter(
                        x => x.type === "movement"
                    );
                    break;
                case 3:
                    var rotationLogs = allLogs.filter(
                        x => x.type === "rotation"
                    );
                    break;
                case 4:
                    var collisionsLogs = allLogs.filter(
                        x => x.type === "collisions" && x.key !== this.log.key
                    );
                    break;
                default:
                    break;
            }
        },
        prepareExcel() {
            this.excel.data = [
                {
                    Element1: this.mesh,
                    Element2: this.log.properties.collider,
                    Vrijeme: (
                        (this.log.properties.time.start - this.lifeTime) /
                        1000
                    ).toFixed(2)
                }
            ];
        },
        emptyCross(allLogs) {
            var collisionTime = this.log.properties.time.start;
            var collider = this.log.properties.collider;
            var categories = this.generateTime(this.lifeTime, collisionTime, 5);
            var series = [
                {
                    name: "Životna linija",
                    data: []
                }
            ];
            for (var i in categories) {
                series[0].data.push(1);
            }
            var collision = (collisionTime - this.lifeTime) / 1000;
            var x = this.findCollision(categories, collision);
            this.graphData = {
                series: series,
                categories: categories,
                annotations: {
                    points: [
                        {
                            x: x,
                            y: 1,
                            marker: {
                                size: 6,
                                fillColor: "#fff",
                                strokeColor: "#FF4560",
                                radius: 2
                            },
                            label: {
                                borderColor: "#FF4560",
                                offsetY: 0,
                                offsetX: 0,
                                textAnchor: "middle",
                                style: {
                                    color: "#fff",
                                    background: "#FF4560"
                                },
                                text: `Sudar (${collider}) ${collision} s`
                            }
                        }
                    ]
                }
            };
        },
        velocityCross(allLogs) {
            var velocityLogs = allLogs.filter(
                x => x.type === "physics" && x.key === "velocity"
            );
            var relevantVelocityLogs = [];
            var beforeLogs = velocityLogs.filter(
                x => x.properties.time.start <= this.lifeTime
            );
            var afterLogs = velocityLogs.filter(
                x => x.properties.time.start >= this.lifeTime
            );

            var series = [
                {
                    name: "x-os",
                    data: []
                },
                {
                    name: "y-os",
                    data: []
                },
                {
                    name: "z-os",
                    data: []
                }
            ];
            var categories = [];
            this.graphData = {
                series: series,
                categories: categories,
                annotations: {
                    points: [
                        {
                            x: x,
                            y: 1,
                            marker: {
                                size: 6,
                                fillColor: "#fff",
                                strokeColor: "#FF4560",
                                radius: 2
                            },
                            label: {
                                borderColor: "#FF4560",
                                offsetY: 0,
                                offsetX: 0,
                                textAnchor: "middle",
                                style: {
                                    color: "#fff",
                                    background: "#FF4560"
                                },
                                text: `Sudar (${collider}) ${collision} s`
                            }
                        }
                    ]
                }
            };
        },
        generateTime(start, end, extra) {
            var time = [];
            var diff = (end - start) / 1000;
            if (diff > extra) {
                var start = Math.floor(diff - extra);
                for (var i = start; i <= diff + extra; i += 0.5) {
                    time.push(i);
                }
            } else {
                for (var i = 0; i <= diff + extra; i += 0.5) {
                    time.push(i);
                }
            }
            return time;
        },
        findCollision(categories, collision) {
            var x = 0;
            for (var i in categories) {
                if (categories[i] == collision) {
                    x = parseInt(i) + 1;
                    break;
                }
                if (categories[i] > collision) {
                    x = parseInt(i) + (collision - Math.floor(collision));
                    break;
                }
            }
            return parseFloat(x);
        }
    }
};
</script>

<style scoped>
</style>
