<template>
    <div id="fizika">
        <div class="block">
            <b-radio v-model="graphType" :native-value="0">Životna linija</b-radio>
        </div>
        <p v-if="graphType === 0">
            <b>Životna linija</b>
            predstavlja promjenu brzine tijela {{this.mesh}} od trenutka njegovog stvaranja (0 s - stvoren element {{this.mesh}}).
        </p>
        <p v-if="graphType === 1"></p>
        <div class="tile is-ancestor">
            <div
                class="tile is-1"
                @click="previous"
                :class="{'disabled': currentGraph == 0, 'my-pagination': true}"
            >
                <b-tooltip type="is-black" label="Prethodna brzina" position="is-right">
                    <a :class="{'disabled': currentGraph == 0, 'my-pagination-button': true}">
                        <span class="icon">
                            <i class="fas fa-angle-left fa-3x"></i>
                        </span>
                    </a>
                </b-tooltip>
            </div>
            <div class="tile">
                <basic-velocity-chart
                    v-if="graphType === 0 && showGraph"
                    :graphData="graphData"
                    style="width: 100%"
                    ref="basic"
                />
            </div>
            <div
                class="tile is-1"
                @click="next"
                :class="{'disabled': currentGraph == numberOfGraphs - 1, 'my-pagination': true}"
            >
                <b-tooltip type="is-black" label="Sljedeća brzina" position="is-left">
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
import BasicVelocityChart from "@/components/experiment/charts/physics/velocity/BasicVelocityChart.vue";
import { mapGetters, mapState } from "vuex";

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
    },
    data() {
        return {
            graphType: 0,
            lifeTime: 0,
            currentGraph: 0,
            numberOfGraphs: 0,
            excel: {
                data: []
            },
            values: [],
            graphData: null,
            showGraph: false
        };
    },
    components: {
        BasicVelocityChart
    },
    mounted() {
        this.currentGraph = 0;
        this.graphType = 0;
        this.numberOfGraphs = 1;
        var allLogs = this.getMeshLog(this.mesh);
        var lifeLog = allLogs.find(x => x.type === "life");
        this.lifeTime = lifeLog.properties.time;
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
            var values = this.values;
            var data = [];
            for (var i in values) {
                var time = (values[i].time - this.lifeTime) / 1000;
                data.push({
                    Vrijeme: `${time.toFixed(2)} s`,
                    PoložajX: `${values[i].velocity.x} m/s`,
                    PoložajY: `${values[i].velocity.y} m/s`,
                    PoložajZ: `${values[i].velocity.z} m/s`
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
            this.values = this.log.properties.values;
            var filteredValues = this.filterVelocity();
            if (this.values.length < 20) {
                filteredValues = this.values;
            }

            var series = filteredValues.map(x => x.velocity);
            var categories = filteredValues.map(x =>
                ((x.time - this.lifeTime) / 1000).toFixed(2)
            );

            if (filteredValues.length > 20) {
                var number = filteredValues.length / 20;
                var diff = number - Math.floor(number);
                if (diff.toFixed(2) >= 0.5) {
                    this.numberOfGraphs = Math.ceil(number);
                } else {
                    this.numberOfGraphs = Math.floor(number);
                }
                var start = this.currentGraph * 20;
                var end = start + 20;
                series = series.slice(start, end);
                categories = categories.slice(start, end);
            } else {
                this.numberOfGraphs = 1;
            }

            this.graphData = {
                series: [
                    {
                        name: "x",
                        data: series.map(x => x.x)
                    },
                    {
                        name: "y",
                        data: series.map(x => x.y)
                    },
                    {
                        name: "z",
                        data: series.map(x => x.z)
                    }
                ],
                categories: categories
            };
        },
        filterVelocity() {
            var result = [];
            var temp = [];
            for (var i in this.values) {
                var realTime = (this.values[i].time - this.lifeTime) / 1000;
                var decimal = realTime - Math.floor(realTime);
                if (
                    decimal.toFixed(1) == 0.5 ||
                    decimal.toFixed(1) == 0.0 ||
                    decimal.toFixed(1) == 1.0
                ) {
                    if (temp.find(x => x == realTime.toFixed(1)) == undefined) {
                        temp.push(realTime.toFixed(1));
                        result.push(this.values[i]);
                    }
                }
            }
            return result;
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
