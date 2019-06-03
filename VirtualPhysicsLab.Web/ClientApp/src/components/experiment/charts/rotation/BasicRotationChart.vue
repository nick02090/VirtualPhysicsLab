<template>
    <apexchart
        ref="chart"
        :height="height"
        type="line"
        :options="charts.line.options"
        :series="charts.line.series"
    ></apexchart>
</template>

<script>
export default {
    name: "BasicRotationChart",
    props: {
        height: {
            type: Number,
            default: 300
        },
        graphData: {
            type: Object,
            required: true
        }
    },
    mounted() {
        this.update();
    },
    methods: {
        async update() {
            let series = [];
            let updatedOptions = {
                xaxis: {
                    type: "category",
                    categories: []
                }
            };

            series = this.graphData.series;
            updatedOptions.xaxis.categories = this.graphData.categories;
            // updatedOptions.annotations = this.graphData.annotations;
            await this.$refs.chart.updateSeries(series, true);
            await this.$refs.chart.updateOptions(updatedOptions, true, true);
        }
    },
    data() {
        return {
            charts: {
                line: {
                    series: [],
                    options: {
                        colors: ["#db2525", "#37964c", "#284e7b"],
                        chart: {
                            type: "line",
                            zoom: {
                                enabled: false
                            },
                            toolbar: {
                                show: false
                            }
                        },
                        dataLabels: {
                            enabled: false
                        },
                        stroke: {
                            curve: "stepline"
                        },
                        grid: {
                            row: {
                                colors: ["#f3f3f3", "transparent"],
                                opacity: 0.5
                            }
                        },
                        tooltip: {
                            enabled: true
                        },
                        xaxis: {
                            categories: []
                        }
                    }
                }
            }
        };
    }
};
</script>
