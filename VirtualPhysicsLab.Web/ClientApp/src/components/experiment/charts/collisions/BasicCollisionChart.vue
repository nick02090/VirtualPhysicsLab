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
    name: "BasicCollisionChart",
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
        let series = [];
        let updatedOptions = {
            xaxis: {
                type: "category",
                categories: []
            },
            annotations: {}
        };

        series = this.graphData.series;
        updatedOptions.xaxis.categories = this.graphData.categories;
        updatedOptions.annotations = this.graphData.annotations;
        this.$refs.chart.updateSeries(series, true);
        this.$refs.chart.updateOptions(updatedOptions, true, true);
    },
    data() {
        return {
            charts: {
                line: {
                    series: [],
                    options: {
                        colors: ["#37964c"],
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
                            curve: "straight"
                        },
                        grid: {
                            row: {
                                colors: ["#f3f3f3", "transparent"],
                                opacity: 0.5
                            }
                        },
                        tooltip: {
                            enabled: false
                        },
                        xaxis: {
                            categories: []
                        },
                        yaxis: {
                            show: false
                        }
                    }
                }
            }
        };
    }
};
</script>

