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
        <div class="tile is-ancestor">
            <div
                class="tile is-1"
                @click="previous"
                :class="{'disabled': currentGraph == 0, 'my-pagination': true}"
            >
                <b-tooltip type="is-black" label="Prethodni pomak" position="is-right">
                    <a :class="{'disabled': currentGraph == 0, 'my-pagination-button': true}">
                        <span class="icon">
                            <i class="fas fa-angle-left fa-3x"></i>
                        </span>
                    </a>
                </b-tooltip>
            </div>
            <div class="tile">
                <basic-movement-chart
                    v-if="graphType === 0 && showGraph"
                    :graphData="graphData"
                    style="width: 100%"
                    ref="basic"
                />
                <velocity-movement-chart
                    v-if="graphType === 1 && showGraph"
                    :graphData="graphData"
                />
                <collision-movement-chart
                    v-if="graphType === 2 && showGraph"
                    :graphData="graphData"
                />
                <rotation-movement-chart
                    v-if="graphType === 3 && showGraph"
                    :graphData="graphData"
                />
            </div>
            <div
                class="tile is-1"
                @click="next"
                :class="{'disabled': currentGraph == numberOfGraphs - 1, 'my-pagination': true}"
            >
                <b-tooltip type="is-black" label="Sljedeći pomak" position="is-left">
                    <a
                        :class="{'disabled': currentGraph == numberOfGraphs - 1, 'my-pagination-button': true}"
                    >
                        <span class="icon">
                            <i class="fas fa-angle-right fa-3x"></i>
                        </span>
                    </a>
                </b-tooltip>
            </div>
        </div>
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
            currentGraph: 0,
            numberOfGraphs: 0,
            startPosition: null,
            positions: [],
            graphData: null,
            showGraph: false,
            excel: {
                data: []
            }
        };
    },
    components: {
        BasicMovementChart,
        VelocityMovementChart,
        CollisionMovementChart,
        RotationMovementChart
    },
    mounted() {
        this.currentGraph = 0;
        this.graphType = 0;
        this.numberOfGraphs = 1;
        var allLogs = this.getMeshLog(this.mesh);
        var lifeLog = allLogs.find(x => x.type === "life");
        this.lifeTime = lifeLog.properties.time;
        this.startPosition = lifeLog.properties.position;
        this.findCross();
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
        async previous() {
            if (this.currentGraph > 0) {
                this.currentGraph -= 1;
                await this.update();
            }
        },
        async next() {
            if (this.currentGraph < this.numberOfGraphs - 1) {
                this.currentGraph += 1;
                await this.update();
            }
        },
        async update() {
            this.findCross();
            this.prepareExcel();
            await this.$refs.basic.update();
            await this.$refs.basic.update();
        },
        prepareExcel() {
            var times = this.generateTime(
                this.lifeTime,
                this.positions[this.numberOfGraphs - 1].time,
                5
            );
            var positions = this.generateValues(times.map(x => x.value));
            var data = [];
            for (var i in positions) {
                data.push({
                    Vrijeme: `${times[i].value} s`,
                    PoložajX: positions[i].x.toFixed(2),
                    PoložajY: positions[i].y.toFixed(2),
                    PoložajZ: positions[i].z.toFixed(2)
                });
            }
            this.excel.data = data;
        },
        findCross() {
            var allLogs = this.getMeshLog(this.mesh);
            switch (this.graphType) {
                case 0:
                    this.emptyCross(allLogs);
                    break;
                default:
                    break;
            }
        },
        emptyCross(allLogs) {
            this.positions = this.log.properties.positions;
            this.numberOfGraphs = this.positions.length;

            var categories = this.generateTime(
                this.lifeTime,
                this.positions[this.numberOfGraphs - 1].time,
                5
            );

            var time =
                (this.positions[this.currentGraph].time - this.lifeTime) / 1000;
            time = this.fixTime(time.toFixed(1));
            var element = categories.find(x => x.value == time);
            var index = categories.indexOf(element);
            var first = index <= 10 ? 0 : index - 10;
            var end = first + 20;
            categories = categories.slice(first, end);
            categories = categories.map(x => x.value);

            var series = this.generateValues(categories);

            this.graphData = {
                series: [
                    {
                        name: "x",
                        data: series.map(x => x.x.toFixed(2))
                    },
                    {
                        name: "y",
                        data: series.map(x => x.y.toFixed(2))
                    },
                    {
                        name: "z",
                        data: series.map(x => x.z.toFixed(2))
                    }
                ],
                categories: categories
            };
        },
        fixTime(time) {
            var decimal = time - Math.floor(time);
            if (decimal.toFixed(2) > 0.5) {
                return Math.ceil(time);
            } else if (decimal.toFixed(2) < 0.5) {
                return Math.floor(time);
            } else {
                return time;
            }
        },
        generateTime(start, end, extra) {
            var time = [];
            var diff = (end - start) / 1000;
            for (var i = 0; i <= diff + extra; i += 0.5) {
                time.push({ value: i });
            }
            return time;
        },
        generateValues(time) {
            var values = [];
            for (var i in time) {
                var t1 = time[i];
                var t2 = time[parseInt(i) + 1];
                if (t2 === undefined) break;
                var find = this.positions.find(
                    x =>
                        (x.time - this.lifeTime) / 1000 <= t2 &&
                        (x.time - this.lifeTime) / 1000 >= t1
                );
                if (find) {
                    values.push(find.value);
                } else {
                    var before = this.positions.filter(
                        x => (x.time - this.lifeTime) / 1000 <= t2
                    );
                    var lastFind = before[before.length - 1];
                    if (lastFind) {
                        values.push(lastFind.value);
                    } else {
                        values.push(this.startPosition);
                    }
                }
            }
            return values;
        }
    }
};
</script>

<style scoped>
.my-pagination {
    cursor: pointer;
}
.my-pagination.disabled {
    cursor: not-allowed;
}
.my-pagination.disabled:hover {
    background-color: white;
}
.my-pagination:hover {
    background-color: #f5f5f9;
}
.my-pagination-button.disabled {
    cursor: not-allowed;
    color: #bfbfbf;
}
.my-pagination-button {
    top: 50%;
    position: absolute;
}
.tile.is-1 {
    width: 3.33%;
}
</style>
