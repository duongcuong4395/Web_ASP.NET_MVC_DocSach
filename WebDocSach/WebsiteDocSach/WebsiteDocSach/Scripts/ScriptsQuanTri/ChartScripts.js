google.load("visualization", "1", { packages: ["corechart"] });
var chartData;
// get the chart data from the server.


$(document).ready(function () {
    // Số luong sản phẩm mà từng khách hàng đã thích
    $.ajax({
        url: "/QuanTri/khDSThich",
        data: "{}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; chartset=utf-8",
        success: function (data) {
            debugger;
            chartData = data;
            // Callback that creates and populates a data table,
            // instantiates the pie chart, passes in the data and draws it.
            //Creates a data table for storing the map data .chart api access this data for displaying
            var data = new google.visualization.DataTable();
            //Adding columns to data table to insert the chart data
            data.addColumn('string', 'hotenkh');
            data.addColumn('number', 'SL');

            //bind the data to the data table using for loop
            for (var i = 0; i < chartData.length; i++) {
                data.addRow([chartData[i].hotenkh, chartData[i].SL]);
            }
            // Instantiate and draw our chart, passing in some options
            var chart = new google.visualization.PieChart(document.getElementById('KHDS-Thich'));
            chart.draw(data,
                  { 
                      is3D: true,
                      width: '100%',
                      height: '100%',
                  });
        },
        // ajax error display
        error: function () {
            alert("Error loading data! Please try again.");
        }
    });

    //Số lượng sách theo chủ đề
    $.ajax({
        url: "/QuanTri/DsSLSachTheoChuDe",
        data: "{}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; chartset=utf-8",
        success: function (data) {
            debugger;
            chartData = data;
            // Callback that creates and populates a data table,
            // instantiates the pie chart, passes in the data and draws it.
            //Creates a data table for storing the map data .chart api access this data for displaying
            var data = new google.visualization.DataTable();
            //Adding columns to data table to insert the chart data
            data.addColumn('string', 'tenchude');
            data.addColumn('number', 'SL');

            //bind the data to the data table using for loop
            for (var i = 0; i < chartData.length; i++) {
                data.addRow([chartData[i].tenchude, chartData[i].SL]);
            }
            // Instantiate and draw our chart, passing in some options
            var chart = new google.visualization.PieChart(document.getElementById('DsSLSachTheoChuDe'));
            chart.draw(data,
                  { 
                      pieHole: 0.5,
                      pieSliceTextStyle: {
                          color: 'black',
                      },
                      legend: 'none'
                  });
        }, 
        // ajax error display
        error: function () {
            alert("Error loading data! Please try again.");
        }
    });





    //Số lượng sách theo chủ đề mới
    $.ajax({
        url: "/QuanTri/DsSLSachTheoChuDe",
        data: "{}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; chartset=utf-8",
        success: function (data) {
            debugger;
            chartData = data;
            // Callback that creates and populates a data table,
            // instantiates the pie chart, passes in the data and draws it.
            //Creates a data table for storing the map data .chart api access this data for displaying
            var data = new google.visualization.DataTable();
            //Adding columns to data table to insert the chart data
            data.addColumn('string', 'tenchude');
            data.addColumn('number', 'SL');

            //bind the data to the data table using for loop
            for (var i = 0; i < chartData.length; i++) {
                data.addRow([chartData[i].tenchude, chartData[i].SL]);
            }
            // Instantiate and draw our chart, passing in some options

            var chart = new google.visualization.ColumnChart(document.getElementById('DsSLSachTheoChuDemoi'));
            

            var options = {
                title: "Density of Precious Metals, in g/cm^3",
                width: 600,
                height: 400,
                bar: { groupWidth: "95%" },
                legend: { position: "none" },
            }; 
            chart.draw(data,options);
        },
        // ajax error display
        error: function () {
            alert("Error loading data! Please try again.");
        }
    });


    $.ajax({
        url: "/QuanTri/SoNguoiXem",
        data: "{}",
        dataType: "json",
        type: "POST",
        contentType: "application/json; chartset=utf-8",
        success: function (data) {
            debugger;
            chartData = data; 
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'NgayXem');
            data.addColumn('number', 'SoNguoiXem');
            
             
            for (var i = 0; i < chartData.length; i++) {
                data.addRow([chartData[i].NgayXem, chartData[i].SoNguoiXem]);
            } 
            var chart = new google.visualization.ColumnChart(document.getElementById('SoNguoiXemab'));
            chart.draw(data,
                  {
                      //title: "Density of Precious Metals, in g/cm^3",
                      //width: 600,
                      //height: 400,
                      //bar: { groupWidth: "95%" },
                      //legend: { position: "none" },
                      title: 'SỐ LƯỢNG NGƯỜI VÀO WEBSITES THEO NGÀY',
                      legend: { position: 'none' },
                      colors: ['green'],
                  });
        }, 
        error: function () {
            alert("Error loading data! Please try again.");
        }
    });

});