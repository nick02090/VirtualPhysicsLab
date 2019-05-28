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
    name: "MovementCollisionChart",
    data() {
        return {
            charts: {
                line: {
                    series: [
                        {
                            name: "",
                            data: [10, 41, 35, 51, 49, 62, 69, 91, 148]
                        }
                    ],
                    options: {
                        annotations: {
                            points: [
                                {
                                    x: 3.5,
                                    y: 35,
                                    marker: {
                                        size: 8,
                                        fillColor: "#fff",
                                        strokeColor: "red",
                                        radius: 2
                                    },
                                    label: {
                                        borderColor: "#FF4560",
                                        offsetY: 0,
                                        style: {
                                            color: "#fff",
                                            background: "#FF4560"
                                        },

                                        text: "Sudar"
                                    }
                                }
                            ]
                        },
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
                            type: "category",
                            categories: [
                                0,
                                0.5,
                                1,
                                1.5,
                                2,
                                2.5,
                                3,
                                3.5,
                                4,
                                4.5,
                                5
                            ]
                        }
                    }
                }
            }
        };
    }
};
</script>
