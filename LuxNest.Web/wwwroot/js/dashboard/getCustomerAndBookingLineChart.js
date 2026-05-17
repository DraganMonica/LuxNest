$(document).ready(function () {
    loadCustomerAndBookingLineChart();
});

function loadCustomerAndBookingLineChart() {
    $(".chart-spinner").show();

    $.ajax({
        url: "/Dashboard/GetMemberAndBookingLineChartData",
        type: 'GET',
        datatype: 'json',
        success: function (data) {
            console.log("Line chart data:", JSON.stringify(data));

            var series = data.series || data.Series;
            var categories = data.categories || data.Categories;

            var bookingsSeries = series[0];
            var customersSeries = series[1];

            renderLineChart("bookingsLineChart", [bookingsSeries], categories, "#28a745");
            renderLineChart("customersLineChart", [customersSeries], categories, "#0d6efd");

            $(".chart-spinner").hide();
        }
    });
}

function renderLineChart(id, series, categories, color) {
    var options = {
        series: series,
        colors: [color],
        chart: {
            type: 'line',
            height: 320,
            toolbar: { show: true }
        },
        stroke: {
            curve: 'smooth',
            width: 3
        },
        markers: {
            size: 4,
            hover: { size: 7 }
        },
        xaxis: {
            categories: categories,
            labels: {
                rotate: -45,
                style: { colors: "#ddd", fontSize: "11px" }
            }
        },
        yaxis: {
            min: 0,
            tickAmount: 4,
            labels: {
                style: { colors: "#fff", fontSize: "13px" }
            }
        },
        legend: {
            labels: { colors: "#fff" }
        },
        tooltip: { theme: 'dark' },
        grid: {
            borderColor: "#333"
        }
    };

    var chart = new ApexCharts(document.querySelector("#" + id), options);
    chart.render();
}
