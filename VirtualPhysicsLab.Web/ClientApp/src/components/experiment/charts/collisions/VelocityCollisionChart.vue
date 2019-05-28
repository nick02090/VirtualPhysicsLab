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
    name: "VelocityCollisionChart",
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
            annotations: {
                points: []
            }
        };

        series = this.graphData.series;
        updatedOptions.xaxis.categories = this.graphData.categories;
        updatedOptions.annotations.points = this.graphData.points;
        // this.$refs.chart.updateSeries(series, true);
        // this.$refs.chart.updateOptions(updatedOptions, true, true);
    },
    data() {
        return {
            charts: {
                line: {
                    series: [
                        {
                            name: "x",
                            data: [
                                3.0,
                                2.67,
                                2.77,
                                2.4,
                                2.2,
                                2.0,
                                2.0,
                                1.98,
                                1.78,
                                1.7
                            ]
                        },
                        {
                            name: "y",
                            data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
                        },
                        {
                            name: "z",
                            data: [
                                0.5,
                                0.41,
                                0.4,
                                0.4,
                                0.38,
                                0.37,
                                0.35,
                                0.3,
                                0.28,
                                0.2
                            ]
                        }
                    ],
                    options: {
                        annotations: {
                            xaxis: [
                                {
                                    x: 2,
                                    borderColor: "#FF4560",
                                    label: {
                                        borderColor: "#FF4560",
                                        offsetY: 0,
                                        offsetX: 0,
                                        textAnchor: "middle",
                                        style: {
                                            color: "#fff",
                                            background: "#FF4560"
                                        },
                                        text: `Sudar (K1) 0.5 s`
                                    }
                                }
                            ]
                        },
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
                            curve: "straight"
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
                            categories: [0, 0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5]
                        }
                    }
                }
            }
        };
    }
};
</script>
