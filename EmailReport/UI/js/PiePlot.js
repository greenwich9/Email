//<script>
//    window.onload = function () {
//        CanvasJS.addColorSet("Color",
//            [//colorSet Array

//                "blue",
//                "green",
//                "red"
//            ]);
//    var chart1 = new CanvasJS.Chart("chartContainer1", {
//        animationEnabled: true,
   
//    theme: "light2", // "light1", "light2", "dark1", "dark2"
//    colorSet : "Color",
   
//	        axisY: {
//        title: "Unique Users"
//},

//	        data: [{
//        type: "column",
//    showInLegend: false,
//    //legendText: "MMbbl = one million barrels",
//    dataPoints: [
//			        {y: @Model.ProcessedCount, label: "Processed" },
//			        {y: @Model.OpenCount,  label: "Open" },
//			        {y: @Model.UniqueClickUser,  label: "Click" }

//]
//}]
//});
//chart1.render();
//var total = @Model.RegionCodeCount.ElementAt(0).Count + @Model.RegionCodeCount.ElementAt(1).Count + @Model.RegionCodeCount.ElementAt(2).Count + @Model.RegionCodeCount.ElementAt(3).Count;
//var AMS = 100 *@Model.RegionCodeCount.ElementAt(0).Count/total;
//var EUR = 100 *@Model.RegionCodeCount.ElementAt(1).Count / total;
//var APJ = 100 *@Model.RegionCodeCount.ElementAt(2).Count / total;
//var Ung = 100 *@Model.RegionCodeCount.ElementAt(3).Count / total;
//CanvasJS.addColorSet("myColor",
//[//colorSet Array

//"blue",
//"green",
//"red"
//]);
//    var chart2 = new CanvasJS.Chart("chartContainer2", {
//        animationEnabled: true,
   
//    theme: "light2",
//    colorSet: "myColor",

//    tooltipFillColor: "rgba(51, 51, 51, 0.55)",
//        data: [{
//        type: "pie",
//    indexLabelFontSize: 20,
//    radius: 150,
//            indexLabel: "{label} - {y}",
//    yValueFormatString: "###0.0\"%\"",
//    click: explodePie,
//    dataPoints: [
//                {y: AMS, label: "AMS" },
//                {y: EUR, label: "EUR" },
//                {y: APJ, label: "APJ" },
//                {y: Ung, label: "Ungrouped" }

//],

//hoverBackgroundColor: [
//    "#CFD4D8",
//    "#B370CF",

//    "#49A9EA"
//]
//}]
//});
//chart2.render();

//    function explodePie(e) {
//        for (var i = 0; i < e.dataSeries.dataPoints.length; i++) {
//            if (i !== e.dataPointIndex)
//        e.dataSeries.dataPoints[i].exploded = false;
//}
//}

//}
//    </script>